-- CREATE TABLE products
--(
--product_id INT NOT NULL PRIMARY KEY IDENTITY,
--product_name VARCHAR(50) NOT NULL,
--product_description VARCHAR(100) NOT NULL,
--product_price float(53) NOT NULL,
--product_instock INT NOT NULL,
--product_image VARCHAR(100) NOT NULL,
--product_category VARCHAR(50) NOT NULL
--);

CREATE TABLE sellers(
seller_id INT NOT NULL PRIMARY KEY IDENTITY,
seller_name VARCHAR(50) NOT NULL,
seller_email VARCHAR(50) NOT NULL UNIQUE,
seller_phone VARCHAR(20) NULL,
seller_address VARCHAR (50) NULL);