const { sequelize, User, ClothingItem, Outfit, OutfitItem, SavedOutfit } = require('./db');
const bcrypt = require('bcrypt');

async function seed() {
  try {
    await sequelize.sync({ force: true });
    console.log('DB synced (force true)');

    const password_hash = await bcrypt.hash('password123', 10);
    const user = await User.create({
      username: 'test_user',
      email: 'test@example.com',
      password_hash,
    });

    const item = await ClothingItem.create({
      clothing_name: 'Test Hat',
      type: 'head',
      material: 'cotton',
      colors: 'red',
      description: 'A test hat',
      image_url: 'https://example.com/hat.jpg',
      added_by_user_id: user.user_id,
      is_approved: true,
      usage_count: 5,
    });

    const outfit = await Outfit.create({
      user_id: user.user_id,
      outfit_name: 'Test Outfit',
      description: 'A sample outfit that includes the test hat',
    });

    const outfitItem = await OutfitItem.create({
      outfit_id: outfit.outfit_id,
      clothing_id: item.clothing_id,
      position: 'head',
    });

    const saved = await SavedOutfit.create({
      user_id: user.user_id,
      outfit_id: outfit.outfit_id,
    });

    console.log('Seeded user', user.toJSON());
    console.log('Seeded item', item.toJSON());
    console.log('Seeded outfit', outfit.toJSON());
    console.log('Seeded outfit item', outfitItem.toJSON());
    console.log('Seeded saved outfit', saved.toJSON());
    process.exit(0);
  } catch (err) {
    console.error('Seeding error', err);
    process.exit(1);
  }
}

seed();
