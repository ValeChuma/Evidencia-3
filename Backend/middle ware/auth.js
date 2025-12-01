const jwt = require('jsonwebtoken');
const { User } = require('../db');

const JWT_SECRET = process.env.JWT_SECRET || 'replace-this-with-a-secret';

async function authenticateJWT(req, res, next) {
  const auth = req.headers.authorization || req.headers.Authorization;
  if (!auth) {
    return res.status(401).json({ success: false, message: 'Missing Authorization header' });
  }
  const parts = auth.split(' ');
  if (parts.length !== 2 || parts[0] !== 'Bearer') return res.status(401).json({ success: false, message: 'Invalid Authorization header' });
  const token = parts[1];
  try {
    const payload = jwt.verify(token, JWT_SECRET);
    // Attach user basic info to request
    const user = await User.findByPk(payload.user_id, { attributes: { exclude: ['password_hash'] } });
    if (!user) return res.status(401).json({ success: false, message: 'Invalid token - user not found' });
    req.user = user;
    next();
  } catch (err) {
    console.error('JWT auth error', err);
    return res.status(401).json({ success: false, message: 'Invalid or expired token' });
  }
}

module.exports = {
  authenticateJWT,
};
