﻿

--CREATE TABLE category(
-- category_id INT NOT NULL PRIMARY KEY IDENTITY,
-- category_name VARCHAR(50) NOT NULL
-- );


--CREATE TABLE products
--(
--    product_id INT NOT NULL PRIMARY KEY IDENTITY,
--    product_name VARCHAR(50) NOT NULL,
--    product_description VARCHAR(100) NOT NULL,
--    product_price FLOAT NOT NULL,
--    product_instock INT NOT NULL,
--    product_image VARCHAR(100) NOT NULL,
--    product_category INT NOT NULL,
--    seller INT NOT NULL,
--    FOREIGN KEY (seller) REFERENCES sellers(seller_id),
--    FOREIGN KEY (product_category) REFERENCES category(category_id)
--);

--CREATE TABLE sellers(
--seller_id INT NOT NULL PRIMARY KEY IDENTITY,
--seller_name VARCHAR(50) NOT NULL,
--seller_email VARCHAR(50) NOT NULL UNIQUE,
--seller_phone VARCHAR(20) NULL,
--seller_address VARCHAR (50) NULL);

--CREATE TABLE orders(
--order_id INT NOT NULL PRIMARY KEY IDENTITY,
--buyer_id INT NOT NULL,
--product_id INT NOT NULL,
--seller_id INT NOT NULL,
--order_price float(53)
--);
--CREATE TABLE users(
--user_id INT NOT NULL PRIMARY KEY IDENTITY,
--user_email VARCHAR(50) NOT NULL UNIQUE,
--user_name VARCHAR(50) NOT NULL,
--user_address VARCHAR(50) NOT NULL,
--user_phone VARCHAR(50) NOT NULL,
--user_password VARCHAR(50) NOT NULL,
--user_role INT NOT NULL
--);


 --CREATE TABLE roles(
 --role_id INT NOT NULL PRIMARY KEY,
 --role_name VARCHAR(50) NOT NULL
 --);
