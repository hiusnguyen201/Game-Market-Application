CREATE DATABASE GMADB;
USE GMADB;

---------- CreateTables ----------
CREATE TABLE Accounts
(
	acc_ID INT PRIMARY KEY AUTO_INCREMENT,
	acc_Username VARCHAR(225) NOT NULL CHECK (acc_Username REGEXP '^[A-Za-z][A-Za-z0-9_-]$'),
	acc_Password VARCHAR(225) NOT NULL CHECK (acc_Password REGEXP '^[^\s]+$'),
	acc_Realname VARCHAR(225) NOT NULL CHECK (acc_Realname REGEXP '^[A-Za-z ]+$'),
    acc_Phone VARCHAR(11) CHECK (acc_Phone REGEXP '^[0-9]{10,20}$'),
    acc_Email VARCHAR(225) UNIQUE NOT NULL CHECK (acc_Email REGEXP '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$'),
    acc_Address VARCHAR(225),
    acc_Money DOUBLE DEFAULT 0,
    acc_CreateDate DATETIME DEFAULT NOW()
);

CREATE INDEX ix_username ON Accounts(acc_Username);
CREATE INDEX ix_email ON Accounts(acc_Email);

CREATE TABLE Carts
(
	cart_ID INT PRIMARY KEY AUTO_INCREMENT,
    acc_ID INT,
    cart_CreateDate DATETIME DEFAULT NOW(),
    cart_TotalPrice DOUBLE NOT NULL,
    cart_Status INT,
    CONSTRAINT fk_Carts_Accounts FOREIGN KEY (acc_ID) references Accounts(acc_ID)
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
    game_Name VARCHAR(225) NOT NULL CHECK (game_Name REGEXP '^[A-Za-z0-9.,:;\'),
    game_Desc TEXT NOT NULL,
    game_Price DOUBLE NOT NULL,
    game_Rating FLOAT,
    game_Size VARCHAR(225) NOT NULL CHECK (game_size REGEXP '^[0-9]+(\.[0-9]+)?( ?[KMGTP]B)?$'),
    game_Status INT,
    game_Discount FLOAT,
    discount_Unit INT CHECK(discount_Unit IN ('%', '$')),
    game_ReleaseDate DATETIME DEFAULT NOW(),
    CONSTRAINT fk_Games_Publishers FOREIGN KEY (publisher_ID) REFERENCES Publishers(publisher_ID)
);

CREATE INDEX ix_gameName ON Games(game_Name);

CREATE TABLE GenreDetails (
	genre_ID INT NOT NULL,
    game_ID INT NOT NULL,
    CONSTRAINT pk_GenreDetails PRIMARY KEY (genre_ID, game_ID),
    CONSTRAINT fk_GenreDetails_Genres FOREIGN KEY (genre_ID) REFERENCES Genres(genre_ID),
    CONSTRAINT fk_GenreDetails_Games FOREIGN KEY (game_ID) REFERENCES Games(game_ID)
);

CREATE TABLE Cartitems
(
	cart_ID INT NOT NULL,
    game_ID INT NOT NULL,
    unit_price DECIMAL(20,2) NOT NULL,
    CONSTRAINT pk_Cartitems PRIMARY KEY (cart_ID, game_ID),
    CONSTRAINT fk_Cartitems_Carts FOREIGN KEY (cart_ID) REFERENCES Carts (cart_ID),
    CONSTRAINT fk_Cartitems_Games FOREIGN KEY (game_ID) REFERENCES Games (game_ID)
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

CREATE TABLE Ownership
(
	acc_ID INT NOT NULL,
    game_ID INT NOT NULL,
    purchase_Date DATETIME,
    CONSTRAINT pk_Ownership PRIMARY KEY (acc_ID, game_ID),
    FOREIGN KEY fk_Ownership_Accounts (acc_ID) REFERENCES Accounts (acc_ID),
    FOREIGN KEY fk_Ownership_Games (game_ID) REFERENCES Games (game_ID)
);

---------- CreateTriggers ----------
DELIMITER $$
	CREATE TRIGGER tg_CheckPriceGame BEFORE INSERT
	ON Games FOR EACH ROW
    BEGIN
		IF NEW.game_Price < 0 
        THEN SIGNAL SQLSTATE '45001' 
        SET MESSAGE_TEXT = "tg_CheckPriceGame: Price must >= 0";
        END IF;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE TRIGGER tg_CheckRatingGame BEFORE INSERT
	ON Games FOR EACH ROW
    BEGIN
		IF NEW.game_Rating < 0 
        THEN SIGNAL SQLSTATE '45001' 
        SET MESSAGE_TEXT = "tg_CheckRatingGame: Rating must >= 0";
        END IF;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE TRIGGER tg_CheckGameDiscount BEFORE INSERT
	ON Games FOR EACH ROW
    BEGIN
		IF NEW.game_Discount < 0 
        THEN SIGNAL SQLSTATE '45001' 
        SET MESSAGE_TEXT = "tg_CheckGameDiscount: Game Discount must >= 0";
        END IF;
    END $$
DELIMITER ;


---------- CreateStoredProcedures ----------
DELIMITER $$
	CREATE PROCEDURE create_account (IN username VARCHAR(225),IN pass VARCHAR(225), IN realName VARCHAR(225), IN phone VARCHAR(11), IN email VARCHAR(225), IN address VARCHAR(225), OUT accountID INT)
    BEGIN
		INSERT INTO Accounts(acc_Username, acc_Pass, acc_Realname, acc_Phone, acc_Email, acc_Address)
        VALUES (username, pass, realName, phone, email, address);
        SELECT MAX(acc_ID) INTO accountID FROM Accounts;
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
	CREATE PROCEDURE get_acc_by_password (IN au VARCHAR(225), IN ap VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts
        WHERE acc_Username = au AND acc_Password = ap;
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

