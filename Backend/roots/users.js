const express = require('express');
const router = express.Router();
const { User } = require('../dv');
const bcrypt = require('bcrypt');

// GET all users
router.get('/', async (req, res) => {
  try {
    const users = await User.findAll({ attributes: { exclude: ['password_hash'] } });
    res.json(users);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// GET user by id
router.get('/:id', async (req, res) => {
  try {
    const user = await User.findByPk(req.params.id, { attributes: { exclude: ['password_hash'] } });
    if (!user) return res.status(404).json({ error: 'User not found' });
    res.json(user);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// POST create user
router.post('/', async (req, res) => {
  try {
    const { username, email, password, is_admin } = req.body;
    const password_hash = await bcrypt.hash(password, 10);
    const user = await User.create({ username, email, password_hash, is_admin });
    const { password_hash: _, ...userSafe } = user.get({ plain: true });
    res.status(201).json(userSafe);
  } catch (err) {
    res.status(400).json({ error: err.message });
  }
});

// PUT update user
router.put('/:id', async (req, res) => {
  try {
    const user = await User.findByPk(req.params.id);
    if (!user) return res.status(404).json({ error: 'User not found' });
    const { username, email, is_admin } = req.body;
    await user.update({ username, email, is_admin });
    const { password_hash: _, ...userSafe } = user.get({ plain: true });
    res.json(userSafe);
  } catch (err) {
    res.status(400).json({ error: err.message });
  }
});

// DELETE user
router.delete('/:id', async (req, res) => {
  try {
    const user = await User.findByPk(req.params.id);
    if (!user) return res.status(404).json({ error: 'User not found' });

    await user.destroy();
    res.status(204).send();
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

module.exports = router;
