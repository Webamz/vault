--CREATE TABLE category (
--    category_id INT NOT NULL PRIMARY KEY IDENTITY,
--    category_name VARCHAR(50) NOT NULL
--);

--CREATE TABLE roles (
--    role_id INT NOT NULL PRIMARY KEY,
--    role_name VARCHAR(50) NOT NULL
--);

--CREATE TABLE users (
--    user_id INT NOT NULL PRIMARY KEY IDENTITY,
--    user_email VARCHAR(50) NOT NULL UNIQUE,
--    user_name VARCHAR(50) NOT NULL,
--    user_address VARCHAR(50) NOT NULL,
--    user_phone VARCHAR(50) NOT NULL,
--    user_password VARCHAR(255) NOT NULL,
--    user_role INT NOT NULL,
--    FOREIGN KEY (user_role) REFERENCES roles(role_id)
--);



--CREATE TABLE sellers (
--    id INT NOT NULL PRIMARY KEY IDENTITY,
--    seller_name VARCHAR(50) NOT NULL,
--    seller_email VARCHAR(50) NOT NULL UNIQUE,
--    seller_phone VARCHAR(20) NULL,
--    seller_address VARCHAR(50) NULL,
--    seller_id INT NOT NULL,
--    FOREIGN KEY (seller_id) REFERENCES users(user_id) ON DELETE CASCADE 
--);


--CREATE TABLE products (
--    product_id INT NOT NULL PRIMARY KEY IDENTITY,
--    product_name VARCHAR(50) NOT NULL,
--    product_description VARCHAR(100) NOT NULL,
--    product_price FLOAT NOT NULL,
--    product_instock INT NOT NULL,
--    product_image VARCHAR(100) NOT NULL,
--    product_category INT NOT NULL,
--    seller INT NOT NULL,
--    FOREIGN KEY (product_category) REFERENCES category(category_id)
--);

--CREATE TABLE orders (
--    order_id INT NOT NULL PRIMARY KEY IDENTITY,
--    customer_id INT NOT NULL,
--    product_id INT,
--    quantity INT,
--    total_price DECIMAL(10,2) NOT NULL,
--    order_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
--    FOREIGN KEY (customer_id) REFERENCES users(user_id) ON DELETE CASCADE, 
--    FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE CASCADE;
--);


