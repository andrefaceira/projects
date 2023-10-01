
CREATE DATABASE stocks;

CREATE USER stocks_admin WITH ENCRYPTED PASSWORD 'stocks_admin';
GRANT ALL PRIVILEGES ON DATABASE stocks TO stocks_admin;

CREATE USER stocks_write WITH ENCRYPTED PASSWORD 'stocks_write';
GRANT USAGE ON DATABASE stocks TO stocks_write;

CREATE USER stocks_read WITH ENCRYPTED PASSWORD 'stocks_read';
GRANT SELECT ON DATABASE stocks TO stocks_read;
