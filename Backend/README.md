# Fashion Application Backend

This backend uses Node.js, Express, and Sequelize ORM to connect to a PostgreSQL database.

## Quick start

1. Install dependencies

```powershell
cd backend
npm install
```

2. Create a Postgres database and copy `.env.example` to `.env` with your credentials.

3. Start server

```powershell
npm run dev
```

This will sync models with the database on startup.

## Project structure
 - `models` - Sequelize models (User, ClothingItem, Outfit, OutfitItem, SavedOutfit)
 - `routes` - Example CRUD routes for users, clothing_items, outfits, saved_outfits

## Notes
 - This scaffolding syncs models to DB using `sequelize.sync()` for simplicity. Use migrations (sequelize-cli) for production.
 
API Endpoints (examples):
 - `GET /api/users` - list users
 - `GET /api/users/:id` - get a user
 - `POST /api/users` - create a user (body: {name, email, password})
 - `PUT /api/users/:id` - update a user
 - `DELETE /api/users/:id` - delete user

 - `GET /api/clothing_items` - list clothing items
 - `GET /api/clothing_items/:id` - get clothing item
 - `POST /api/clothing_items` - create clothing item (body: { clothing_name, type, material, colors, description, image_url, added_by_user_id })
 - `PUT /api/clothing_items/:id` - update clothing item
 - `DELETE /api/clothing_items/:id` - delete clothing item

 - `GET /api/outfits` - list outfits
 - `GET /api/outfits/:id` - get outfit with items
 - `POST /api/outfits` - create outfit (body: { user_id, outfit_name, description })
 - `PUT /api/outfits/:id` - update outfit
 - `DELETE /api/outfits/:id` - delete outfit
 - `POST /api/outfits/:id/items` - add item to outfit (body: { clothing_id, position })
 - `DELETE /api/outfits/:id/items/:itemId` - delete outfit item

 - `GET /api/saved_outfits` - list saved outfits
 - `POST /api/saved_outfits` - save outfit (body: { user_id, outfit_id })
 - `DELETE /api/saved_outfits/:id` - delete saved outfit
 - `GET /api/me` - get authenticated user (token required)
Frontend header:
 - A reusable `components/header.html` is served via `/components/header.html` and injected into pages.
 - The header shows Login/Signup when not authenticated, and Username + Logout when authenticated. The logout clears the local token and redirects to the login page.

Configuration:
 - Copy `.env.example` to `.env` and set Postgres credentials and `PORT`.
 - The environment supports either separate `DB_HOST`, `DB_USER`, `DB_PASS`, `DB_NAME` (default `DB_NAME` in example is `postgres@Local`), or set a `DATABASE_URL` connection string to override those.
	 Example:
	 - `DB_NAME=postgres@Local`
	 - Or `DATABASE_URL=postgres://postgres:postgres@localhost:5432/postgres`
