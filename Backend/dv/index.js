const { Sequelize } = require('sequelize');
require('dotenv').config();
const path = require('path');

// Use SQLite for simplicity in development
const sequelize = new Sequelize({
  dialect: 'sqlite',
  storage: path.join(__dirname, '..', 'database.sqlite'),
  logging: process.env.NODE_ENV === 'development' ? console.log : false,
});

// Import models
const User = require('../Models/User')(sequelize);

module.exports = {
  sequelize,
  User,
};
