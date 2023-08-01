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
    publisher_Name VARCHAR(225) NOT NULL UNIQUE
);

CREATE TABLE Genres
(
	genre_ID INT PRIMARY KEY AUTO_INCREMENT,
    genre_Name VARCHAR(225) NOT NULL UNIQUE
);

CREATE TABLE Games
(
	game_ID INT PRIMARY KEY AUTO_INCREMENT,
    publisher_ID INT NOT NULL,
    game_Name VARCHAR(225) UNIQUE NOT NULL,
    game_Desc TEXT NOT NULL,    
    game_Price DOUBLE NOT NULL,
    game_Rating FLOAT,
    game_Size VARCHAR(225) NOT NULL,
    game_Discount FLOAT,
    game_ReleaseDate DATETIME DEFAULT NOW(),
    CONSTRAINT fk_Games_Publishers FOREIGN KEY (publisher_ID) REFERENCES Publishers(publisher_ID)
);

CREATE TABLE GameGenres (
	genre_ID INT NOT NULL,
    game_ID INT NOT NULL,
    CONSTRAINT fk_GameGenres_Genres FOREIGN KEY (genre_ID) REFERENCES Genres(genre_ID),
    CONSTRAINT fk_GameGenres_Games FOREIGN KEY (game_ID) REFERENCES Games(game_ID),
    CONSTRAINT pk_GameGenres PRIMARY KEY (genre_ID, game_ID)
);

CREATE TABLE OrderDetails
(
	order_ID INT NOT NULL,
    game_ID INT NOT NULL,
    unit_price DECIMAL(20,2) NOT NULL,
    CONSTRAINT fk_OrderDetails_Orders FOREIGN KEY (order_ID) REFERENCES Orders (order_ID),
    CONSTRAINT fk_OrderDetails_Games FOREIGN KEY (game_ID) REFERENCES Games (game_ID),
    CONSTRAINT pk_OrderDetails PRIMARY KEY (order_ID, game_ID)
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

DELIMITER $$
    CREATE PROCEDURE get_game_by_id (IN gid INT)
    BEGIN
        SELECT * 
        FROM Games AS g
        INNER JOIN Publishers AS p
        ON p.publisher_ID = g.publisher_ID
        INNER JOIN GameGenres AS gg
        ON g.game_ID = gg.genre_ID
        INNER JOIN Genres AS gen
        ON gen.genre_ID = gg.genre_ID
        WHERE g.game_ID = gid;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_all_games ()
    BEGIN
        SELECT * 
        FROM Games AS g
        INNER JOIN Publishers AS p
        ON p.publisher_ID = g.publisher_ID
        INNER JOIN GameGenres AS gg
        ON g.game_ID = gg.game_ID
        INNER JOIN Genres AS gen
        ON gen.genre_ID = gg.genre_ID;
    END $$
DELIMITER ;

INSERT INTO publishers(publisher_Name)
VALUES ("SEGA"), ("Square Enix"), ("Bandai Namco Entertainment"),("Devolver Digital"),("Laush Studio"),
("PlayWay"),("Artifex Mundi"),("THQ Nordic"),("Daedalic Entertainment"),("Nacon"),
("Electronic Arts"),("Team17 Software"),("Capcom Entertainment"),("2K Games"),("Dnovel"),
("tinyBuild"),("Gamera Games"),("GrabTheGames"),("Tero Lunkka"),("DIG Publishing"),
("Focus Entertainment"),("Xbox Game Studios"),("Cute Hannah's Games"),("Microids"),("Alawar Entertainment");

INSERT INTO games(publisher_ID, game_Name, game_Desc, game_Price, game_Rating, game_Size, game_Discount, game_ReleaseDate)
VALUES (1, "8-Bit Bayonetta", "dont know", 134000, 0.44, "60MB", null, "2017-04-01"),
(1, "A Total War Saga: TROY", "dont know", 734000, null, "26.00GB", null, "2021-09-02");

INSERT INTO genres(genre_Name)
VALUES ("Action"), ("Casual"), ("Strategy"), ("Simulation");

INSERT INTO gamegenres(game_ID, genre_ID)
VALUES (1, 1), (1, 2),
(2, 1), (2, 3), (2, 4);

call get_all_games ();

