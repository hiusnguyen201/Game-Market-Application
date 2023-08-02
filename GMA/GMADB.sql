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
    game_Rating INT,
    game_Size VARCHAR(225) NOT NULL,
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

CREATE VIEW get_all_games AS
SELECT g.game_ID, g.game_Name, g.game_Desc, g.game_Price, g.game_Rating, g.game_Size, g.game_ReleaseDate, p.publisher_ID, p.publisher_Name, GROUP_CONCAT(gen.genre_ID) AS genre_ID, GROUP_CONCAT(gen.genre_Name) AS genre_Name
FROM Games AS g
INNER JOIN Publishers AS p ON p.publisher_ID = g.publisher_ID
INNER JOIN GameGenres AS gg ON gg.game_ID = g.game_ID
INNER JOIN Genres AS gen ON gen.genre_ID = gg.genre_ID
GROUP BY g.game_ID;

DELIMITER $$
    CREATE PROCEDURE get_game_by_id (IN gid INT)
    BEGIN
        SELECT * 
        FROM get_all_games AS gag
        WHERE gag.game_ID = gid;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_game_by_Key (IN kw VARCHAR(225))
    BEGIN
        SELECT * 
        FROM get_all_games AS gag
        WHERE gag.game_name LIKE CONCAT('%', kw, '%');
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_game_by_GenIdKey (IN kw VARCHAR(225), IN genid INT)
    BEGIN
        SELECT * 
        FROM get_all_games AS gag
        WHERE gag.game_name LIKE CONCAT('%', kw, '%')
        AND FIND_IN_SET(genid, gag.genre_ID);
    END $$
DELIMITER ;




INSERT INTO publishers(publisher_Name)
VALUES ("Valve"), ("Larian Studios"), ("Gearbox Publishing"),
("Electronic Arts"), ("SgtOkiDoki"), ("Rockstar Games"), ("Activision"),
("Behaviour Interactive Inc."), ("NetEase Games Global"), ("KRAFTON, Inc."),
("Bethesda Softworks"), ("Digital Extremes"), ("MINTROCKET"), ("Ubisoft"),
("Facepunch Studios"), ("CAPCOM Co., Ltd."), ("Gaijin Distribution KFT"),
("Bungie"), ("Insomniac Games");



INSERT INTO games(publisher_ID, game_Name, game_Desc, game_Price, game_Rating, game_Size, game_ReleaseDate)
VALUES (1, "Counter-Strike: Global Offensive", "A competitive FPS game where players join either terrorist or counter-terrorist teams to complete objectives and engage in tactical combat.", 0, 88, "15GB", "2012-08-22"),
(2, "Baldur's Gate 3", "A role-playing video game set in the Dungeons & Dragons universe, offering a rich narrative and turn-based combat in a vast open world", 990000, 89, "150GB", "2020-10-7"),
(3, "Remnant II", "A cooperative third-person shooter set in a post-apocalyptic world filled with dangerous creatures and ancient technology.", 830000, 80, "80GB", "2023-07-25"),
(4, "Apex Legends™", "A free-to-play battle royale game, where players form squads, choose unique characters with special abilities, and compete to be the last team standing in intense, fast-paced matches set in a futuristic world.", 0, 81, "56GB", "2020-11-5"),
(5, "BattleBit Remastered", "BattleBit Remastered aims for a chaotic, massively multiplayer online first-person shooter experience.", 253500, 90, "2GB", "2023-6-15"),
(6, "Red Dead Redemption 2", "An action-adventure game set in the American Wild West, following the story of outlaw Arthur Morgan as he navigates a vast open world filled with moral choices and intense gunfights.", 1000000, 90, "150GB", "2019-12-6"),
(6, "Grand Theft Auto V", "An open-world action-adventure game developed by Rockstar Games, offering players a sprawling fictional city to explore, multiple playable characters, and a mix of thrilling missions and activities.GTV 5 is an open-world action-adventure game developed by Rockstar Games, offering players a sprawling fictional city to explore, multiple playable characters, and a mix of thrilling missions and activities.", 311000, 86, "72GB", "2015-6-14"),
(7, "Call of Duty®: Modern Warfare® II", "A critically acclaimed first-person shooter, offering a gritty and realistic military experience with an engaging single-player campaign and a robust multiplayer mode filled with various maps, weapons, and customization options.", 1799000, 60, "125GB", "2022-10-28"),
(8, "Dead by Daylight", "An asymmetrical horror multiplayer game, where players take on the roles of either deadly killers or survivors, trying to outwit each other in chilling and suspenseful matches set in various eerie environments.", 230000, 80, "50GB", "2016-6-14"),
(9, "NARAKA: BLADEPOINT", "A fast-paced multiplayer battle royale game with a unique melee-focused combat system, set in a mystical Asian-inspired world where players fight for survival and glory.", 0, 74, "35GB", "2021-8-12"),
(10, "PUBG: BATTLEGROUNDS", "A popular battle royale game where up to 100 players parachute onto an island, scavenge for weapons and equipment, and fight to be the last person or team standing in intense, tactical battles.", 0, 57, "40GB", "2017-12-21"),
(11, "The Elder Scrolls® Online", "An expansive MMORPG set in the vast world of Tamriel, offering players the chance to explore diverse regions, embark on epic quests, and engage in dynamic, multiplayer adventures.", 450000, 83, "95GB", "2014-4-4"),
(1, "Team Fortress 2", "A class-based multiplayer first-person shooter with a cartoonish art style, offering fast-paced team-based gameplay and a variety of humorous characters to choose from.", 0, 93, "15GB", "2007-10-10"),
(12, "Warframe", "A free-to-play cooperative third-person shooter, set in a futuristic sci-fi universe, where players control powerful warframes to complete missions, defeat enemies, and acquire new weapons and abilities.", 0, 86, "50GB", "2013-3-26"),
(13, "DAVE THE DIVER", "Explore and unravel the mysteries in the depths of the Blue Hole by day and run a successful exotic sushi restaurant by night.It’s easy to get hooked on the satisfying gameplay loop!", 260000, 97, "10GB", "2023-6-28"),
(4, "F1® 23", "Be the last to brake in EA SPORTS™ F1® 23, the official video game of the 2023 FIA Formula One World Championship™.", 1590000, 85, "80GB", "2023-6-16"),
(14, "Tom Clancy's Rainbow Six® Siege", "A tactical multiplayer first-person shooter, where players engage in intense, close-quarters combat as specialized operators with unique abilities in a variety of game modes.", 330000, 86, "61GB", "2015-12-2"),
(1, "Dota 2", "A free-to-play multiplayer online battle arena (MOBA) game, where two teams of five players each compete to destroy the opposing team's Ancient in a strategic and action-packed gameplay.", 0, 82, "60GB", "2013-7-10"),
(15, "Rust", "A multiplayer survival game set in a harsh open world, where players must gather resources, build structures, and fight against other players and environmental challenges to survive and thrive.", 510000, 87, "25GB", "2018-2-9"),
(16, "Street Fighter™ 6", "A classic 2D fighting game franchise, where players select iconic characters and engage in intense martial arts battles, using special moves and combos to defeat opponents in one-on-one matches.", 1322000, 89, "60GB", "2023-6-1"),
(4, "The Sims™ 4", "A life simulation game, where players create and control virtual characters, build homes, and guide their daily lives in a sandbox-style environment, offering endless creative possibilities and storytelling opportunities.", 0, 87, "51GB", "2014-9-2"),
(7, "Call of Duty®: Black Ops III", "A fast-paced first-person shooter, set in a futuristic world with advanced technology and cybernetic enhancements, offering a gripping campaign, multiplayer action, and cooperative zombie mode.", 1363500, 86, "100GB", "2015-11-6"),
(17, "War Thunder", "A free-to-play vehicular combat MMO, featuring intense battles with aircraft, ground vehicles, and naval vessels set in various historical periods, offering a mix of realistic simulation and arcade-style gameplay.", 0, 75, "95GB", "2013-8-15"),
(18, "Destiny 2", "A sci-fi online multiplayer shooter and RPG, where players fight as Guardians to defend Earth from various threats, featuring cooperative raids, player versus environment (PvE) missions, and competitive player versus player (PvP) modes.", 0, 82, "105GB", "2019-10-1"),
(19, "Ratchet & Clank: Rift Apart", "A visually stunning action-adventure game, where players join the heroic duo on an interdimensional journey, battling enemies and using inventive weaponry to save the multiverse from a sinister threat.", 1399000, 85, "75GB", "2023-7-26");


INSERT INTO genres(genre_Name)
VALUES ("Action"), ("Adventure"), ("Strategy"), ("RPG"), ("Multiplayer"),
("Casual"), ("Indie"), ("Simulation"), ("Racing"), ("Sports");



INSERT INTO gamegenres(game_ID, genre_ID)
VALUES (1, 1),
(2, 2),(2, 3),(2, 4),
(3, 1), (3, 2), (3, 4),
(4, 1), (4, 2),
(5, 1), (5, 5),
(6, 1), (6, 2),
(7, 1), (7, 2),
(8, 1),
(9, 1),
(10, 1), (10, 2), (10, 5),
(11, 1), (11, 2), (11, 5),
(12, 1), (12, 2), (12, 4), (12, 5),
(13, 1),
(14, 1), (14, 4),
(15, 2), (15, 4), (15, 6), (15, 7), (15, 8),
(16, 9), (16, 10),
(17, 1),
(18, 1), (18, 3),
(19, 1), (19, 2), (19, 4), (19, 5), (19, 7),
(20, 1), (20, 2),
(21, 2), (21, 6), (21, 8),
(22, 1), (22, 2),
(23, 1), (23, 5), (23, 8),
(24, 1), (24, 2),
(25, 1), (25, 2);

call get_game_by_Key("a");