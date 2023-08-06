CREATE DATABASE GMADB;
USE GMADB;

CREATE TABLE Accounts
(
	acc_ID INT PRIMARY KEY AUTO_INCREMENT,
	acc_Username VARCHAR(225) UNIQUE NOT NULL CHECK (acc_Username REGEXP '^[^\\s][a-zA-Z0-9_-]{3,}$'),
	acc_Password TEXT NOT NULL,
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

DELIMITER $$
    CREATE PROCEDURE get_genre_by_id (IN genid VARCHAR(225))
    BEGIN
        SELECT * 
        FROM genres AS gen
        WHERE gen.genre_Id = genid;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_genre_by_Key (IN kw VARCHAR(225))
    BEGIN
        SELECT * 
        FROM genres AS gen
        WHERE gen.genre_Name LIKE CONCAT('%', kw, '%');
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE create_order (IN aid INT, IN otp DOUBLE, IN os INT, OUT oid INT )
    BEGIN
		INSERT INTO orders (acc_ID, order_TotalPrice, order_Status)
        VALUES (aid, otp, os)
        SET oid = LAST_INSERT_ID();
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_order_by_id (IN oid INT, IN aid INT )
    BEGIN
		SELECT *
		FROM orders 
        WHERE order_ID = oid AND acc_ID = aid;
	END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_all_order (IN aid INT)
    BEGIN
		SELECT *
		FROM orders 
        WHERE acc_ID = aid;
	END $$
DELIMITER ;

INSERT INTO publishers(publisher_Name)
VALUES ("Valve"), ("Larian Studios"), ("Gearbox Publishing"),
("Electronic Arts"), ("SgtOkiDoki"), ("Rockstar Games"), ("Activision"),
("Behaviour Interactive Inc."), ("NetEase Games Global"), ("KRAFTON, Inc."),
("Bethesda Softworks"), ("Digital Extremes"), ("MINTROCKET"), ("Ubisoft"),
("Facepunch Studios"), ("CAPCOM Co., Ltd."), ("Gaijin Distribution KFT"),
("Bungie"), ("Insomniac Games"), ("Newnight"), ("FromSoftware Inc."), ("Pearl Abyss"),
("SCS Software"), ("Kinetic Games"), ("Xbox Game Studios"), ("CD PROJEKT RED"),
("Crytek"), ("The Indie Stone"), ("ConcernedApe"), ("Frontier Developments"), ("Focus Entertainment"),
("Paradox Interactive"), ("VOID Interactive"), ("Warner Bros"), ("Axolot Games");



INSERT INTO games(publisher_ID, game_Name, game_Desc, game_Price, game_Rating, game_Size, game_ReleaseDate)
VALUES (1, "Counter-Strike: Global Offensive", "Counter-Strike: Global Offensive (CS: GO) expands upon the team-based action gameplay that it pioneered when it was launched 19 years ago. CS: GO features new maps, characters, weapons, and game modes, and delivers updated versions of the classic CS content (de_dust2, etc.).", 0, 88, "15GB", "2012-08-22"),
(2, "Baldur's Gate 3", "Baldur’s Gate 3 is a story-rich, party-based RPG set in the universe of Dungeons & Dragons, where your choices shape a tale of fellowship and betrayal, survival and sacrifice, and the lure of absolute power.", 990000, 89, "150GB", "2020-10-7"),
(3, "Remnant II", "Remnant II pits survivors of humanity against new deadly creatures and god-like bosses across terrifying worlds. Play solo or co-op with two other friends to explore the depths of the unknown to stop an evil from destroying reality itself.", 830000, 80, "80GB", "2023-07-25"),
(4, "Apex Legends", "Apex Legends is the award-winning, free-to-play Hero Shooter from Respawn Entertainment. Master an ever-growing roster of legendary characters with powerful abilities, and experience strategic squad play and innovative gameplay in the next evolution of Hero Shooter and Battle Royale.", 0, 81, "56GB", "2020-11-5"),
(5, "BattleBit Remastered", "BattleBit Remastered is a low-poly, massive multiplayer FPS, supporting 254 players per server. Battle on a near-fully destructible map with various vehicles!", 253500, 90, "2GB", "2023-6-15"),
(6, "Red Dead Redemption 2", "Winner of over 175 Game of the Year Awards and recipient of over 250 perfect scores, RDR2 is the epic tale of outlaw Arthur Morgan and the infamous Van der Linde gang, on the run across America at the dawn of the modern age. Also includes access to the shared living world of Red Dead Online.", 1000000, 90, "150GB", "2019-12-6"),
(6, "Grand Theft Auto V", "Grand Theft Auto V for PC offers players the option to explore the award-winning world of Los Santos and Blaine County in resolutions of up to 4k and beyond, as well as the chance to experience the game running at 60 frames per second.", 311000, 86, "72GB", "2015-6-14"),
(7, "Call of Duty®: Modern Warfare® II", "Call of Duty®: Modern Warfare® II drops players into an unprecedented global conflict that features the return of the iconic Operators of Task Force 141.", 1799000, 60, "125GB", "2022-10-28"),
(8, "Dead by Daylight", "Dead by Daylight is a multiplayer (4vs1) horror game where one player takes on the role of the savage Killer, and the other four players play as Survivors, trying to escape the Killer and avoid being caught and killed.", 230000, 80, "50GB", "2016-6-14"),
(9, "NARAKA: BLADEPOINT", "Dive into the legends of the Far East in NARAKA: BLADEPOINT; team up with friends in fast-paced melee fights for a Battle Royale experience unlike any other. Find your playstyle with a varied cast of heroes with unique skills. More than 20 million players have already joined the fray, play free now!", 0, 74, "35GB", "2021-8-12"),
(10, "PUBG: BATTLEGROUNDS", "Play PUBG: BATTLEGROUNDS for free. Land on strategic locations, loot weapons and supplies, and survive to become the last team standing across various, diverse Battlegrounds. Squad up and join the Battlegrounds for the original Battle Royale experience that only PUBG: BATTLEGROUNDS can offer.", 0, 57, "40GB", "2017-12-21"),
(11, "The Elder Scrolls® Online", "Join over 22 million players in the award-winning online multiplayer RPG and experience limitless adventure in a persistent Elder Scrolls world. Battle, craft, steal, or explore, and combine different types of equipment and abilities to create your own style of play. No game subscription required.", 450000, 83, "95GB", "2014-4-4"),
(1, "Team Fortress 2", "Nine distinct classes provide a broad range of tactical abilities and personalities. Constantly updated with new game modes, maps, equipment and, most importantly, hats!", 0, 93, "15GB", "2007-10-10"),
(12, "Warframe", "Awaken as an unstoppable warrior and battle alongside your friends in this story-driven free-to-play online action game", 0, 86, "50GB", "2013-3-26"),
(13, "DAVE THE DIVER", "DAVE THE DIVER is a casual, singleplayer adventure RPG featuring deep-sea exploration and fishing during the day and sushi restaurant management at night. Join Dave and his quirky friends as they seek to uncover the secrets of the mysterious Blue Hole.", 260000, 97, "10GB", "2023-6-28"),
(4, "F1® 23", "Be the last to brake in EA SPORTS™ F1® 23, the official video game of the 2023 FIA Formula One World Championship™. A new chapter in the thrilling 'Braking Point' story mode delivers high-speed drama and heated rivalries.", 1590000, 85, "80GB", "2023-6-16"),
(14, "Tom Clancy's Rainbow Six® Siege", "Tom Clancy's Rainbow Six® Siege is an elite, tactical team-based shooter where superior planning and execution triumph.", 330000, 86, "61GB", "2015-12-2"),
(1, "Dota 2", "Every day, millions of players worldwide enter battle as one of over a hundred Dota heroes. And no matter if it's their 10th hour of play or 1,000th, there's always something new to discover. With regular updates that ensure a constant evolution of gameplay, features, and heroes, Dota 2 has taken on a life of its own.", 0, 82, "60GB", "2013-7-10"),
(15, "Rust", "The only aim in Rust is to survive. Everything wants you to die - the island’s wildlife and other inhabitants, the environment, other survivors. Do whatever it takes to last another night.", 510000, 87, "25GB", "2018-2-9"),
(16, "Street Fighter 6", "Here comes Capcom’s newest challenger! Street Fighter™ 6 launches worldwide on June 2nd, 2023 and represents the next evolution of the Street Fighter™ series! Street Fighter 6 spans three distinct game modes, including World Tour, Fighting Ground and Battle Hub.", 1322000, 89, "60GB", "2023-6-1"),
(4, "The Sims 4", "Play with life and discover the possibilities. Unleash your imagination and create a world of Sims that’s wholly unique. Explore and customize every detail from Sims to homes–and much more.", 0, 87, "51GB", "2014-9-2"),
(7, "Call of Duty®: Black Ops III", "Call of Duty®: Black Ops III Zombies Chronicles Edition includes the full base game plus the Zombies Chronicles content expansion.", 1363500, 86, "100GB", "2015-11-6"),
(17, "War Thunder", "War Thunder is the most comprehensive free-to-play, cross-platform, MMO military game dedicated to aviation, armoured vehicles, and naval craft, from the early 20th century to the most advanced modern combat units. Join now and take part in major battles on land, in the air, and at sea.", 0, 75, "95GB", "2013-8-15"),
(18, "Destiny 2", "Destiny 2 is an action MMO with a single evolving world that you and your friends can join anytime, anywhere, absolutely free.", 0, 82, "105GB", "2019-10-1"),
(19, "Ratchet & Clank: Rift Apart", "Blast your way through an interdimensional adventure with Ratchet and Clank – now on PC! Help them take on an evil emperor from another reality as you jump between action-packed worlds and beyond at hyper-speed!", 1399000, 85, "75GB", "2023-7-26"),
(20, "Sons Of The Forest", "Sent to find a missing billionaire on a remote island, you find yourself in a cannibal-infested hellscape. Craft, build, and struggle to survive, alone or with friends, in this terrifying new open-world survival horror simulator.", 385000, 83, "20GB", "2023-2-24"),
(21, "ELDEN RING", "THE NEW FANTASY ACTION RPG. Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.", 898000, 92, "60GB", "2022-2-25"),
(22, "Black Desert", "Played by over 20 million Adventurers - Black Desert Online is an open-world, action MMORPG. Experience intense, action-packed combat, battle massive world bosses, fight alongside friends to siege and conquer castles, and train in professions such as fishing, trading, crafting, cooking, and more!", 80000, 76, "39GB", "2019-9-18"),
(23, "Euro Truck Simulator 2", "Travel across Europe as king of the road, a trucker who delivers important cargo across impressive distances! With dozens of cities to explore, your endurance, skill and speed will all be pushed to their limits.", 87500, 97, "12GB", "2012-10-18"),
(23, "American Truck Simulator", "Experience legendary American trucks and deliver various cargoes across sunny California, sandy Nevada, and the Grand Canyon State of Arizona. American Truck Simulator takes you on a journey through the breathtaking landscapes and widely recognized landmarks around the States.", 87500, 96, "7GB", "2016-2-2"),
(24, "Phasmophobia", "Phasmophobia is a 4 player online co-op psychological horror. Paranormal activity is on the rise and it’s up to you and your team to use all the ghost-hunting equipment at your disposal in order to gather as much evidence as you can.", 160000, 96, "21GB", "2020-9-19"),
(25, "Forza Horizon 5", "Your Ultimate Horizon Adventure awaits! Explore the vibrant open world landscapes of Mexico with limitless, fun driving action in the world’s greatest cars. Conquer the rugged Sierra Nueva in the ultimate Horizon Rally experience. Requires Forza Horizon 5 game, expansion sold separately.", 990000, 88, "110GB", "2021-11-9"),
(4, "EA SPORTS FIFA 23", "FIFA 23 brings The World’s Game to the pitch, with HyperMotion2 Technology, men’s and women’s FIFA World Cup™, women’s club teams, cross-play features**, and more.", 1090000, 54,"100GB","2022-9-29"),
(26, "The Witcher® 3: Wild Hunt", "You are Geralt of Rivia, mercenary monster slayer. Before you stands a war-torn, monster-infested continent you can explore at will. Your current contract? Tracking down Ciri — the Child of Prophecy, a living weapon that can alter the shape of the world.",390000, 96,"50GB","2015-5-18"),
(26, "Cyberpunk 2077", "Cyberpunk 2077 is an open-world, action-adventure RPG set in the dark future of Night City — a dangerous megalopolis obsessed with power, glamor, and ceaseless body modification.", 990000, 80,"70GB","2020-12-10"),
(27, "Hunt: Showdown", "Hunt: Showdown is a high-stakes, tactical PvPvE first-person shooter. Hunt for bounties in the infested Bayou, kill nightmarish monsters and outwit competing hunters - alone or in a group - with your glory, gear, and gold on the line.", 499000, 83,"50GB","2019-8-27"),
(28, "Project Zomboid", "Project Zomboid is the ultimate in zombie survival. Alone or in MP: you loot, build, craft, fight, farm and fish in a struggle to survive. A hardcore RPG skillset, a vast map, massively customisable sandbox and a cute tutorial raccoon await the unwary. So how will you die? All it takes is a bite..", 260000, 94,"6.8GB","2013-11-8"),
(29, "Stardew Valley", "You've inherited your grandfather's old farm plot in Stardew Valley. Armed with hand-me-down tools and a few coins, you set out to begin your new life. Can you learn to live off the land and turn these overgrown fields into a thriving home?", 165000, 98, "500MB","2016-2-27"),
(30, "Planet Zoo", "Build a world for wildlife in Planet Zoo. From the developers of Planet Coaster and Zoo Tycoon comes the ultimate zoo sim. Construct detailed habitats, manage your zoo, and meet authentic living animals who think, feel and explore the world you create around them.", 721500, 90,"16GB","2019-11-5"),
(7, "Call of Duty®: Black Ops Cold War", "Black Ops Cold War, the direct sequel to Call of Duty®: Black Ops, will drop fans into the depths of the Cold War’s volatile geopolitical battle of the early 1980s.", 1490000, 78, "175GB", "2023-3-8"),
(31, "Atomic Heart", "In a mad and sublime utopian world, take part in explosive encounters. Adapt your fighting style to each opponent, use your environment and upgrade your equipment to fulfill your mission. If you want to reach the truth, you'll have to pay in blood.", 750000, 84,"90GB","2023-2-21"),
(16, "MONSTER HUNTER RISE", "Rise to the challenge and join the hunt! In Monster Hunter Rise, the latest installment in the award-winning and top-selling Monster Hunter series, you’ll become a hunter, explore brand new maps and use a variety of weapons to take down fearsome monsters as part of an all-new storyline.", 881000, 86,"36GB","2022-1-13"),
(32, "Cities: Skylines", "Cities: Skylines is a modern take on the classic city simulation. The game introduces new game play elements to realize the thrill and hardships of creating and maintaining a real city whilst expanding on some well-established tropes of the city building experience.", 325000, 93,"4GB", "2015-3-10"),
(33, "Ready or Not", "Ready or Not is an intense, tactical, first-person shooter that depicts a modern-day world in which SWAT police units are called to defuse hostile and confronting situations.", 310000, 91, "90GB","2021-12-18"),
(34, "Hogwarts Legacy", "Hogwarts Legacy is an immersive, open-world action RPG. Now you can take control of the action and be at the center of your own adventure in the wizarding world.", 990000, 92,"85GB", "2023-2-11"),
(25, "Sea of Thieves 2023 Edition", "Celebrate five years since Sea of Thieves' launch with this special edition, including a copy of the game with all permanent content added since launch, plus a 10,000 gold bonus and a selection of Hunter cosmetics: the Hunter Cutlass, Pistol, Compass, Hat, Jacket and Sails.", 95000, 90,"50GB","2020-6-3"),
(35, "Raft", "Raft throws you and your friends into an epic oceanic adventure! Alone or together, players battle to survive a perilous voyage across a vast sea! Gather debris, scavenge reefs and build your own floating home, but be wary of the man-eating sharks!", 188000, 93, "10GB", "2022-6-20");

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
(25, 1), (25, 2),
(26, 1), (26, 2), (26, 7), (26, 8),
(27, 1), (27, 4),
(28, 1), (28, 2), (28, 5), (28, 8), (28, 3),
(29, 7), (29, 8),
(30, 7), (30, 8),
(31, 1), (31, 7),
(32, 1), (32, 2), (32, 9), (32, 8), (32, 10),
(33, 8), (33, 10),
(34, 4),
(35, 4),
(36, 1),
(37, 7), (37, 4), (37, 8),
(38, 7), (38, 4), (38, 8),
(39, 6), (39, 8), (39, 3),
(40, 1),
(41, 1), (41, 2), (41, 4),
(42, 1),
(43, 8), (43, 3),
(44, 1), (44, 2), (44, 7),
(45, 1), (45, 2), (45, 4),
(46, 1), (46, 2),
(47, 2), (47, 4), (47, 8);