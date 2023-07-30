CREATE DATABASE GMADB;
USE GMADB;

CREATE TABLE Accounts
(
	acc_ID INT PRIMARY KEY AUTO_INCREMENT,
	acc_Username VARCHAR(225) UNIQUE NOT NULL CHECK (acc_Username REGEXP '^[^\\s][a-zA-Z0-9_-]{3,}$'),
	acc_Password VARCHAR(225) NOT NULL,
	acc_Realname VARCHAR(225) NOT NULL CHECK (acc_Realname REGEXP '^[A-Za-z ]{2,}$'),
    acc_Email VARCHAR(225) UNIQUE NOT NULL CHECK (acc_Email REGEXP '^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'),
    acc_Address VARCHAR(225) NOT NULL,
    acc_Money DOUBLE DEFAULT 0,
    acc_CreateDate DATETIME DEFAULT NOW()
);

CREATE TABLE Orders
(
	order_ID INT PRIMARY KEY AUTO_INCREMENT,
    acc_ID INT,
    order_TotalPrice DOUBLE NOT NULL,
    order_CreateDate DATETIME DEFAULT NOW(),
    order_Status INT,
    CONSTRAINT fk_Orders_Accounts FOREIGN KEY (acc_ID) references Accounts(acc_ID)
);

CREATE TABLE Publishers
(
	publisher_ID INT PRIMARY KEY AUTO_INCREMENT,
    publisher_Name VARCHAR(225) NOT NULL CHECK (publisher_Name REGEXP '^[A-Za-z .-]+$')
);

CREATE TABLE Genres
(
	genre_ID INT PRIMARY KEY AUTO_INCREMENT,
    genre_Name VARCHAR(225) NOT NULL CHECK (genre_Name REGEXP '^[A-Za-z -]+$')
);

CREATE TABLE Games
(
	game_ID INT PRIMARY KEY AUTO_INCREMENT,
    publisher_ID INT NOT NULL,
    game_Name VARCHAR(225) UNIQUE NOT NULL CHECK (game_Name REGEXP '^[A-Za-z0-9\s\-]+$'),
    game_Desc TEXT NOT NULL,    
    game_Price DOUBLE NOT NULL,
    game_Rating FLOAT,
    game_Size VARCHAR(225) NOT NULL CHECK (game_size REGEXP '^\d+(\.\d+)?\s*(KB|MB|GB)$'),
    game_Discount FLOAT,
    game_ReleaseDate DATETIME DEFAULT NOW(),
    CONSTRAINT fk_Games_Publishers FOREIGN KEY (publisher_ID) REFERENCES Publishers(publisher_ID)
);

CREATE TABLE GameGenres (
	genre_ID INT NOT NULL,
    game_ID INT NOT NULL,
    CONSTRAINT pk_GameGenres PRIMARY KEY (genre_ID, game_ID),
    CONSTRAINT fk_GameGenres_Genres FOREIGN KEY (genre_ID) REFERENCES Genres(genre_ID),
    CONSTRAINT fk_GameGenres_Games FOREIGN KEY (game_ID) REFERENCES Games(game_ID)
);

CREATE TABLE OrderDetails
(
	order_ID INT NOT NULL,
    game_ID INT NOT NULL,
    unit_price DECIMAL(20,2) NOT NULL,
    CONSTRAINT pk_OrderDetails PRIMARY KEY (order_ID, game_ID),
    CONSTRAINT fk_OrderDetails_Orders FOREIGN KEY (order_ID) REFERENCES Orders (order_ID),
    CONSTRAINT fk_OrderDetails_Games FOREIGN KEY (game_ID) REFERENCES Games (game_ID)
);



DELIMITER $$
	CREATE PROCEDURE create_account (IN aun VARCHAR(225),IN apw VARCHAR(225), IN arn VARCHAR(225), IN ae VARCHAR(225), IN aa VARCHAR(225), OUT aid INT)
    BEGIN
		INSERT INTO Accounts(acc_Username, acc_Password, acc_Realname, acc_Email, acc_Address)
        VALUES (aun, apw, arn, ae, aa);
        SET aid = LAST_INSERT_ID();
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_acc_by_username (IN aun VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts
        WHERE acc_username = aun;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_acc_by_email (IN ae VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts
        WHERE acc_Email = ae;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_account_login (IN aun VARCHAR(225), IN apw VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts
        WHERE acc_Username = aun AND acc_Password = apw;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE update_acc_Money (IN aid INT, IN money DOUBLE)
    BEGIN
        UPDATE Accounts
        SET acc_Money = money
        WHERE acc_ID = aid;
    END $$
DELIMITER ;