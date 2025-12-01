-- Create role mi_usuario with password
CREATE ROLE mi_usuario WITH LOGIN PASSWORD 'mi_contraseña';

-- Create database mi_basedatos owned by mi_usuario
CREATE DATABASE mi_basedatos OWNER mi_usuario;

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE mi_basedatos TO mi_usuario;

-- Display roles to confirm
\du
