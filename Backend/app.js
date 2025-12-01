const express = require('express');
const dotenv = require('dotenv');

dotenv.config();

const { sequelize } = require('./dv');
const cors = require('cors');
const path = require('path');
const userRoutes = require('./roots/users');
const authRoutes = require('./roots/auth');


const app = express();
app.use(express.json());
app.use(cors());

// Serve static assets (CSS/JS) from wwwroot at /wwwroot
app.use('/wwwroot', express.static(path.join(__dirname, '..', 'wwwroot')));
// Serve HTML views directly from /
app.use('/', express.static(path.join(__dirname, '..', 'views')));
// Serve components (header) so client can fetch them
app.use('/components', express.static(path.join(__dirname, '..', 'components')));
// Serve assets like images
app.use('/assets', express.static(path.join(__dirname, '..', 'assets')));

app.get('/', (req, res) => {
  res.json({ message: 'Fashion Application API' });
});

app.use('/api/users', userRoutes);
// Auth routes: support old/named endpoints /api/login & /api/signup
app.use('/api', authRoutes);

const PORT = process.env.PORT || 3000;

async function start() {
  try {
    await sequelize.authenticate();
    console.log('Connection to DB has been established successfully.');

    // Synchronize models. For production, prefer migrations.
    await sequelize.sync({ alter: true });
    console.log('All models were synchronized successfully.');

    app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
  } catch (err) {
    console.error('Unable to connect to the database:', err);
    process.exit(1);
  }
}

start();
