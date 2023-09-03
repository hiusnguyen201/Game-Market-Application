CREATE DATABASE GMADB;
USE GMADB;

CREATE TABLE Accounts
(
	id INT PRIMARY KEY AUTO_INCREMENT,
	username VARCHAR(100) UNIQUE NOT NULL CHECK (username REGEXP '^[^\\s][a-zA-Z0-9_-]{3,100}$'),
	password TEXT NOT NULL,
	realname VARCHAR(100) NOT NULL CHECK (realname REGEXP '^[A-Za-z ]{2,100}$'),
    email VARCHAR(100) UNIQUE NOT NULL CHECK (email REGEXP '^[^\\s][a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3,100}$'),
    address VARCHAR(100) NOT NULL,
    money DOUBLE DEFAULT 0,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE Orders
(
	id INT PRIMARY KEY AUTO_INCREMENT,
    account_id INT NOT NULL,
    totalprice DOUBLE NOT NULL,
    status INT NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    CONSTRAINT orders_acc_id_foreign FOREIGN KEY (account_id) references Accounts(id)
);

CREATE TABLE Publishers
(
	id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Genres
(
	id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Games
(
	id INT PRIMARY KEY AUTO_INCREMENT,
    publisher_id INT NOT NULL,
    name VARCHAR(255) UNIQUE NOT NULL,
    description TEXT NOT NULL,    
    price DOUBLE NOT NULL,
    rating INT NOT NULL,
    size VARCHAR(20) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    CONSTRAINT games_publisher_id_foreign FOREIGN KEY (publisher_id) REFERENCES Publishers(id)
);

CREATE TABLE GamesGenres (
	genre_ID INT NOT NULL,
    game_ID INT NOT NULL,
    CONSTRAINT GamesGenres_genre_id_foreign FOREIGN KEY (genre_ID) REFERENCES Genres(id),
    CONSTRAINT GamesGenres_game_id_foreign FOREIGN KEY (game_ID) REFERENCES Games(id),
    PRIMARY KEY (genre_ID, game_ID)
);

CREATE TABLE OrdersGames
(
	order_ID INT NOT NULL,
    game_ID INT NOT NULL,
    CONSTRAINT OrdersGames_order_id_foreign FOREIGN KEY (order_ID) REFERENCES Orders (id),
    CONSTRAINT OrdersGames_game_id_foreign FOREIGN KEY (game_ID) REFERENCES Games (id),
    PRIMARY KEY (order_ID, game_ID)
);

DELIMITER $$
	CREATE PROCEDURE create_account (IN aun VARCHAR(225),IN apw VARCHAR(225), IN arn VARCHAR(225), IN ae VARCHAR(225), IN aa VARCHAR(225), OUT aid INT)
    BEGIN
		INSERT INTO Accounts(username, password, realname, email, address)
        VALUES (aun, apw, arn, ae, aa);
        SET aid = LAST_INSERT_ID();
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_acc_by_id (IN aid INT)
    BEGIN
		SELECT *
        FROM Accounts AS a
        WHERE a.id = aid;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_acc_by_username (IN aun VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts AS a
        WHERE a.username = aun;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_acc_by_email (IN ae VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts AS a
        WHERE a.email = ae;
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_account_login (IN aun VARCHAR(225), IN apw VARCHAR(225))
    BEGIN
		SELECT *
        FROM Accounts AS a
        WHERE a.username = aun AND a.password = apw;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE update_acc_Money (IN aid INT, IN mn DOUBLE, OUT res INT)
    BEGIN
        UPDATE Accounts
        SET money = mn, updated_at = NOW()
        WHERE id = aid;
        
        IF(ROW_COUNT() = 0) THEN 
			SET res = 0;
		ELSE 
			SET res = aid;
		END IF;
    END $$
DELIMITER ;

CREATE VIEW get_all_games AS
SELECT g.id AS game_id, g.name AS game_name, g.description, g.price, g.rating, g.size, g.created_at, p.id AS publisher_id, p.name AS publisher_name, GROUP_CONCAT(gen.id) AS genre_id, GROUP_CONCAT(gen.name) AS genre_name
FROM Games AS g
INNER JOIN Publishers AS p ON p.id = g.publisher_id
INNER JOIN GamesGenres AS gg ON gg.game_ID = g.id
INNER JOIN Genres AS gen ON gen.id = gg.genre_ID
GROUP BY g.id;

DELIMITER $$
    CREATE PROCEDURE get_game_by_id (IN gid INT)
    BEGIN
        SELECT *    
        FROM get_all_games AS gag
        WHERE gag.game_id = gid;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_game_by_GenIdKey (IN kw VARCHAR(225), IN genid INT)
    BEGIN
		IF(genid = 1) THEN
			SELECT * 
			FROM get_all_games AS gag
			WHERE gag.game_name LIKE CONCAT('%', kw, '%');
        ELSE
			SELECT * 
			FROM get_all_games AS gag
			WHERE gag.game_name LIKE CONCAT('%', kw, '%')
			AND FIND_IN_SET(genid, gag.genre_id);
		END IF;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_genre_by_id (IN genid VARCHAR(225))
    BEGIN
        SELECT * 
        FROM genres AS gen
        WHERE gen.id = genid;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_all_genres ()
    BEGIN
        SELECT * 
        FROM genres;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE create_order (IN aid INT, IN otp DOUBLE, IN os INT, OUT oid INT )
    BEGIN
		INSERT INTO orders (account_id, totalprice, status)
        VALUES (aid, otp, os);
        SET oid = LAST_INSERT_ID();
    END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE create_order_details (IN oid INT, IN gid INT, OUT id INT)
    BEGIN
        DECLARE count INT DEFAULT 0;
        SELECT COUNT(*) INTO count FROM orders AS o WHERE o.id = oid;
        
        IF(count = 0) THEN
			SET id = 0;
		ELSE
			INSERT INTO OrdersGames (order_ID, game_ID)
			VALUES(oid, gid);
			SET id = oid;
		END IF;
    END $$
DELIMITER ;

DELIMITER $$
    CREATE PROCEDURE get_order_by_id (IN oid INT)
    BEGIN
		SELECT o.id, o.account_id, o.totalprice, o.created_at, o.status, GROUP_CONCAT(og.game_id) AS game_id
		FROM orders AS o
        INNER JOIN ordersgames AS og ON o.id = og.order_ID
        WHERE o.id = oid
        GROUP BY o.id;
	END $$
DELIMITER ;

DELIMITER $$
	CREATE PROCEDURE get_all_order (IN aid INT)
	BEGIN
		SELECT o.id, o.account_id, o.totalprice, o.created_at, o.status, GROUP_CONCAT(og.game_id) AS game_id
		FROM orders AS o
		INNER JOIN ordersgames AS og ON o.id = og.order_ID
		WHERE o.account_id = aid
		GROUP BY o.id;
	END $$
DELIMITER ;

INSERT INTO publishers(name)
VALUES ("Valve"), ("Larian Studios"), ("Gearbox Publishing"),
("Electronic Arts"), ("SgtOkiDoki"), ("Rockstar Games"), ("Activision"),
("Behaviour Interactive Inc."), ("NetEase Games Global"), ("KRAFTON, Inc."),
("Bethesda Softworks"), ("Digital Extremes"), ("MINTROCKET"), ("Ubisoft"),
("Facepunch Studios"), ("CAPCOM Co., Ltd."), ("Gaijin Distribution KFT"),
("Bungie"), ("Insomniac Games"), ("Newnight"), ("FromSoftware Inc."), ("Pearl Abyss"),
("SCS Software"), ("Kinetic Games"), ("Xbox Game Studios"), ("CD PROJEKT RED"),
("Crytek"), ("The Indie Stone"), ("ConcernedApe"), ("Frontier Developments"), ("Focus Entertainment"),
("Paradox Interactive"), ("VOID Interactive"), ("Warner Bros"), ("Axolot Games"), ("SCKR Games");



INSERT INTO games(publisher_id, name, description, price, rating, size, created_at)
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
(4, "F1® 23", "Be the last to brake in EA SPORTS F1® 23, the official video game of the 2023 FIA Formula One World Championship. A new chapter in the thrilling 'Braking Point' story mode delivers high-speed drama and heated rivalries.", 1590000, 85, "80GB", "2023-6-16"),
(14, "Tom Clancy's Rainbow Six® Siege", "Tom Clancy's Rainbow Six® Siege is an elite, tactical team-based shooter where superior planning and execution triumph.", 330000, 86, "61GB", "2015-12-2"),
(1, "Dota 2", "Every day, millions of players worldwide enter battle as one of over a hundred Dota heroes. And no matter if it's their 10th hour of play or 1,000th, there's always something new to discover. With regular updates that ensure a constant evolution of gameplay, features, and heroes, Dota 2 has taken on a life of its own.", 0, 82, "60GB", "2013-7-10"),
(15, "Rust", "The only aim in Rust is to survive. Everything wants you to die - the island’s wildlife and other inhabitants, the environment, other survivors. Do whatever it takes to last another night.", 510000, 87, "25GB", "2018-2-9"),
(16, "Street Fighter 6", "Here comes Capcom’s newest challenger! Street Fighter 6 launches worldwide on June 2nd, 2023 and represents the next evolution of the Street Fighter series! Street Fighter 6 spans three distinct game modes, including World Tour, Fighting Ground and Battle Hub.", 1322000, 89, "60GB", "2023-6-1"),
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
(4, "EA SPORTS FIFA 23", "FIFA 23 brings The World’s Game to the pitch, with HyperMotion2 Technology, men’s and women’s FIFA World Cup, women’s club teams, cross-play features**, and more.", 1090000, 54,"100GB","2022-9-29"),
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
(35, "Raft", "Raft throws you and your friends into an epic oceanic adventure! Alone or together, players battle to survive a perilous voyage across a vast sea! Gather debris, scavenge reefs and build your own floating home, but be wary of the man-eating sharks!", 188000, 93, "10GB", "2022-6-20"),
(36, "Only Up!", "Have you ever wanted to walk up to the clouds? Embark on an exciting journey in Only UP! Exploring a huge world full of secrets and mysteries, you have to get as high as possible, and the most interesting starts above the clouds...", 115000, 71, "6GB", "2023-5-24"),
(1, "Aperture Desk Job", "Introducing Aperture Desk Job — a free playable short made for the Steam Deck, set in the universe of the modestly popular Portal games.", 0, 94, "4GB", "2022-3-2"),
(1, "Portal 2", "The 'Perpetual Testing Initiative' has been expanded to allow you to design co-op puzzles for you and your friends!", 30000, 98, "8GB", "2011-4-19"),
(1, "Alien Swarm", "Co-operative multiplayer game and complete code base available for free.", 0, 94, "2.5GB", "2010-7-19"),
(1, "Left 4 Dead 2", "Set in the zombie apocalypse, Left 4 Dead 2 (L4D2) is the highly anticipated sequel to the award-winning Left 4 Dead, the #1 co-op game of 2008. This co-operative action horror FPS takes you and your friends through the cities, swamps and cemeteries of the Deep South, from Savannah to New Orleans across five expansive campaigns.", 142000, 97, "13GB", "2009-11-17"),
(1, "Left 4 Dead", "From Valve (the creators of Counter-Strike, Half-Life and more) comes Left 4 Dead, a co-op action horror game for the PC and Xbox 360 that casts up to four players in an epic struggle for survival against swarming zombie hordes and terrifying mutant monsters.", 142000, 96, "7.5GB", "2008-11-17"),
(1, "Half-Life Deathmatch: Source", "Half-Life Deathmatch: Source is a recreation of the first multiplayer game set in the Half-Life universe. Features all the classic weapons and most-played maps, now running on the Source engine.", 142000, 73, "4GB", "2006-5-1"),
(1, "Half-Life 2", "1998. HALF-LIFE sends a shock through the game industry with its combination of pounding action and continuous, immersive storytelling. Valve's debut title wins more than 50 game-of-the-year awards on its way to being named 'Best PC Game Ever' by PC Gamer, and launches a franchise with more than eight million retail units sold worldwide.", 142000, 97, "6500MB", "2004-11-16"),
(2, "Divinity: Original Sin 2 - Definitive Edition", "The critically acclaimed RPG that raised the bar, from the creators of Baldur's Gate 3. Gather your party. Master deep, tactical combat. Venture as a party of up to four - but know that only one of you will have the chance to become a God.", 350000, 95, "60GB", "2017-9-14"),
(2, "Divinity: Original Sin - Enhanced Edition", "Gather your party and get ready for the kick-ass new version of GameSpot's PC Game of the Year 2014. With hours of new content, new game modes, full voiceovers, split-screen multiplayer, and thousands of improvements, there's never been a better time to explore the epic world of Rivellon!", 310000, 89, "10000MB", "2015-10-27"),
(2, "Divinity: Dragon Commander", "You are the Dragon Commander. Your mission it is to reunite a broken empire and become the new emperor. Success depends entirely on your ability to efficiently rule your empire, build invincible armies and lead them to victory.", 310000, 72, "15GB", "2013-8-6"),
(2, "Divinity II: Developer's Cut", "Dragons: they have been hunted, they have been slain, but now the hour to strike back has come.", 188000, 83, "15GB", "2012-10-30"),
(2, "Beyond Divinity", "Beyond Divinity is the follow-up to the award-winning Divine Divinity.", 80000, 52, "2300MB", "2004-4-27"),
(2, "Divine Divinity", "Divine Divinity is an epic role-playing game with hack-and-slash action, offering a huge world to explore and thousands of items to investigate, trade and use.", 80000, 87, "1.7GB", "2002-9-20"),
(3, "Have a Nice Death", "Have a Nice Death is a 2D action roguelike where you play as an overworked Death, whose employees have run rampant, completely throwing off the balance of souls - and his vacation plans. In order to restore order, you'll have to grab your trusty scythe and show your employees who's boss.", 365000, 85, "2GB", "2023-3-22"),
(3, "Blanc", "Blanc is an artistic cooperative adventure that follows the journey of a wolf cub and a fawn stranded in a vast, snowy wilderness. They must come together in an unlikely partnership to find their families.", 249000, 73, "2GB", "2023-2-15"),
(3, "Eyes in the Dark", "Fight with light against the dark in this 'roguelight' platformer starring Victoria Bloom! Drive back the darkness that's overrun Bloom Manor and conquer swarms of creatures as you discover powerful new items to upgrade your arsenal. Are you ready to enter the ever-changing manor?", 165000, 75, "800MB", "2022-7-14"),
(3, "Godfall Ultimate Edition", "Aperion is on the precipice of ruin. You are the last of the Valorian knights, god-like warriors able to equip Valorplates, legendary armor sets that transform wielders into unstoppable masters of melee combat. Ascend in Godfall, the first-of-its-kind, looter-slasher, melee action-RPG.", 373000, 63, "50GB", "2022-4-8"),
(3, "Tribes of Midgard", "A vibrant blend of survival and action RPG for 1-10 players! Craft legendary items, grow your home base and embark on an epic journey through procedural realms to face towering creatures hel-bent on unleashing Ragnarök. Valhalla can wait, Einherjar!", 188000, 76, "8GB", "2021-7-28"),
(3, "Torchlight III", "In Torchlight III, Novastraia is again under threat of invasion and it’s up to you to defend against the Netherim and its allies. Gather your wits and brave the frontier to find fame, glory, and new adventures!", 465000, 47, "10GB", "2020-10-13"),
(3, "Risk of Rain 2", "Escape a chaotic alien planet by fighting through hordes of frenzied monsters – with your friends, or on your own. Combine loot in surprising ways and master each character until you become the havoc you feared upon your first crash landing.", 220000, 96, "4GB", "2020-8-11"),
(3, "Remnant: From the Ashes", "The world has been thrown into chaos by an ancient evil from another dimension. As one of the last remnants of humanity, you must set out alone or alongside up to two other survivors to face down hordes of deadly enemies to try to carve a foothold, rebuild, and retake what was lost.", 376000, 85, "3GB", "2019-8-20"),
(3, "Duke Nukem 3D: 20th Anniversary World Tour", "Frag like it’s 1996 – this time with even more asses to kick! Join the world’s greatest action hero in Duke Nukem 3D: 20th Anniversary World Tour as he saves Earth once again, kicking alien ass and saving babes across the globe along the way.", 188000, 84, "1200MB", "2016-10-12"),
(3, "Livelock", "Livelock is a co-operative top-down shooter where you play solo or with up to two allies to break the cycle of infinite war between machines. As one of the remaining Capital Intellects, your role is to unlock Eden and revive humanity.", 120000, 80, "8GB", "2016-8-30"),
(3, "Homeworld: Deserts of Kharak", "A ground-based RTS prequel to the classic Homeworld games. Assemble your fleet and lead them to victory on the shifting sands of Kharak in this compelling strategy game for PC from Blackbird Interactive.", 390000,83, "8GB" , "2016-2-20"),
(3, "Homeworld Remastered Collection", "Experience the epic space strategy games that redefined the RTS genre. Control your fleet and build an armada across more than 30 single-player missions. Choose unit types, fleet formations and flight tactics for each strategic situation.", 480000, 88, "20GB" , "2015-2-25"),
(4, "Immortals of Aveum", "Immortals of Aveum is a single-player first person magic shooter that tells the story of Jak as he joins an elite order of battlemages to save a world on the edge of abyss.", 990000, 66, "110GB", "2023-8-22"),
(4, "Madden NFL 24", "Experience the newest iteration of FieldSENSE in Madden NFL 24. More realistic character movement and smarter AI gives you control to play out your gameplay strategy with the confidence to dominate any opponent.", 1090000, 31, "50GB", "2023-8-18"),
(4, "Super Mega Baseball 4", "Super Mega Baseball 4 is where the Legends play. The signature combo of arcade-inspired style and immersive gameplay returns with over 200 baseball Legends and a grand slam of presentation upgrades.", 990000, 73, "25GB", "2023-6-2"),
(4, "Mass Effect 2 (2010) Edition", "Are you prepared to lose everything to save the galaxy? You'll need to be, Commander Shepard. It's time to bring together your greatest allies and recruit the galaxy's fighting elite to continue the resistance against the invading Reapers.", 690000, 60, "22GB", "2010-1-26"),
(4, "STAR WARS Jedi: Survivor", "The story of Cal Kestis continues in STAR WARS Jedi: Survivor, a galaxy-spanning, third-person, action-adventure game.", 1090000, 63, "155GB", "2023-4-28"),
(4, "EA SPORTS PGA TOUR", "The exclusive home of the Majors, EA SPORTS PGA TOUR features Pure Strike for superior golf gameplay, powered by ShotLink®, and unrivaled access to the world’s most exclusive golf courses.", 1090000, 54, "100GB", "2023-4-7"),
(4, "WILD HEARTS", "Master ancient tech to hunt down giant beasts. WILD HEARTS is a twist on the hunting genre where technology gives you a fighting chance against giant nature-infused beasts. Hunt alone or with friends.", 1090000, 47, "80GB" ,"2023-2-16"),
(4, "Dead Space", "The sci-fi survival-horror classic returns, completely rebuilt to offer an even more immersive experience — including visual, audio, and gameplay improvements — while staying faithful to the original game’s thrilling vision.", 990000, 91, "50GB" ,"2023-1-27"),
(4, "Need for Speed Unbound", "Race to the top, definitely don’t flop. Outsmart the cops, and enter weekly qualifiers for The Grand: the ultimate street race. Pack your garage with precision-tuned, custom rides and light up the streets with your style.", 1090000, 62, "50GB" ,"2022-12-2"),
(4, "Madden NFL 23", "Play your way into the history books. Control your impact with every decision in all-new ways. Call the shots in Franchise with free agency and trade logic updates, leave a legacy in Face of the Franchise: The League, and assemble the most powerful roster in all of Madden Ultimate Team.", 990000, 52, "50GB", "2022-8-19"),
(4, "Plants vs. Zombies Garden Warfare 2: Deluxe Edition", "Ready the Peashooters and prepare for the craziest, funniest shooter in the universe: Plants vs. Zombies Garden Warfare 2.", 700000, 89, "40GB", "2022-5-17"),
(4, "GRID Legends", "GRID Legends delivers thrilling wheel-to-wheel motorsport action. Create dream race events, hop into live races, experience a dramatic virtual production story, and embrace the sensation of spectacular racing.", 990000, 74, "50GB" ,"2022-2-24"),
(4, "Battlefield 2042", "Never back down in Season 5: New Dawn. Battlefield 2042 is a first-person shooter that marks the return to the iconic all-out warfare of the franchise.", 990000, 40, "100GB","2021-11-19"),
(4, "Lost in Random", "Play the odds in Lost in Random, a gothic-fairy-tale-inspired action-adventure where every citizen’s fate is determined by a roll of the dice.", 690000, 90, "20GB" ,"2021-9-10"),
(4, "Madden NFL 22", "Madden NFL 22 is where gameday happens. All-new features in Franchise include staff management, an enhanced scenario engine, and weekly strategy. Share avatar progress and player class between Face of The Franchise and The Yard with unified progression.", 990000, 57 ,"50GB" ,"2021-8-20"),
(4, "Mass Effect Legendary Edition", "The Mass Effect Legendary Edition includes single-player base content and over 40 DLC from the highly acclaimed Mass Effect, Mass Effect 2, and Mass Effect 3 games, including promo weapons, armors, and packs — remastered and optimized for 4K Ultra HD.", 990000, 91, "120GB","2021-5-14"),
(4, "It Takes Two Friend's Pass", "Download Friend's Pass* and play It Takes Two with a friend for free. Just grab a friend who owns the full game, download Friend's Pass for free, and play together!", 0, 88, "50GB", "2021-5-27"),
(4, "It Takes Two", "Embark on the craziest journey of your life in It Takes Two. Invite a friend to join for free with Friend’s Pass and work together across a huge variety of gleefully disruptive gameplay challenges. Winner of GAME OF THE YEAR at the Game Awards 2021.", 790000, 95, "50GB","2021-5-26"),
(4, "Medal of Honor: Above and Beyond", "Take the fight to war-torn Europe in Medal of Honor: Above and Beyond. The franchise returns to its roots complete with a deep single-player campaign, multiplayer modes, and powerful interviews with survivors and veterans of the Second World War. (VR Title)" ,790000, 64, "120GB" ,"2020-12-11"),
(4, "Need for Speed Hot Pursuit Remastered", "Feel the thrill of the chase and the rush of escape behind the wheels of the world’s hottest high-performance cars in Need for Speed Hot Pursuit Remastered– a heart-pumping, socially competitive racing experience.", 700000, 81, "45GB" ,"2020-11-6"),
(4, "DIRT 5", "DIRT 5 is a fun, amplified, off-road arcade racing experience created by Codemasters. Blaze a trail on routes across the world, covering gravel, ice, snow and sand, with a roster of cars ranging from rally icons to trucks, to GT heroes.", 480000, 59, "60GB","2020-11-5"),
(4, "Battlefield V", "This is the ultimate Battlefield V experience. Enter mankind’s greatest conflict with the complete arsenal of weapons, vehicles, and gadgets plus the best customization content of Year 1 and 2.", 890000,70,"50GB","2018-11-9"),
(4, "STAR WARS: Squadrons", "Master the art of starfighter combat in the authentic piloting experience STAR WARS: Squadrons. Feel the adrenaline of first-person multiplayer space dogfights alongside your squadron, and buckle up in a thrilling STAR WARS story.", 950000, 70, "40GB","2020-10-1"),
(4, "STAR WARS: The Old Republic", "STAR WARS: The Old Republic is a free-to-play MMORPG that puts you at the center of your own story-driven saga. Play as a Jedi, Sith, Bounty Hunter, or one of many other iconic STAR WARS roles in the galaxy far, far away over three thousand years before the classic films.", 0, 89, "75GB", "2011-12-20"),
(4, "Rocket Arena", "Rockets rule everything in Rocket Arena, an explosive 3v3 shooter where you’re never out of the action. Master your hero’s unique rockets and abilities to rule the arena and become a champion. Let’s Rocket!", 120000,72,"30GB" ,"2020-7-14"),
(4, "Need for Speed Most Wanted", "The open-world action in Need for Speed Most Wanted gives you the freedom to drive your way. Hit jumps and shortcuts, switch cars, lie low or head for terrain that plays to your vehicle’s unique strengths. Fight your way past cops and rivals using skill, high-end car tech and tons of nitrous.", 470000, 79, "20GB","2012-10-30"),
(4, "A Way Out", "A Way Out is an exclusively co-op adventure where you play the role of one of two prisoners making their daring escape from prison.", 700000, 86, "25GB", "2018-3-23"),
(4, "Titanfall® 2", "Respawn Entertainment gives you the most advanced titan technology in its new, single player campaign & multiplayer experience. Combine & conquer with new titans & pilots, deadlier weapons, & customization and progression systems that help you and your titan flow as one unstoppable killing force.", 700000, 94, "45GB", "2016-10-28");


INSERT INTO genres(name)
VALUES ("None"), ("Action"), ("Adventure"), ("Strategy"), ("RPG"), ("Multiplayer"),
("Casual"), ("Indie"), ("Simulation"), ("Racing"), ("Sports");


INSERT INTO gamesgenres(game_id, genre_id)
VALUES (1, 2),
(2, 3),(2, 4),(2, 5),
(3, 2), (3, 3), (3, 5),
(4, 2), (4, 3),
(5, 2), (5, 6),
(6, 2), (6, 3),
(7, 2), (7, 3),
(8, 2),
(9, 2),
(10, 2), (10, 3), (10, 6),
(11, 2), (11, 3), (11, 6),
(12, 2), (12, 3), (12, 5), (12, 6),
(13, 2),
(14, 2), (14, 5),
(15, 3), (15, 5), (15, 7), (15, 8), (15, 9),
(16, 10),(16, 11),
(17, 2),
(18, 2), (18, 4),
(19, 2), (19, 3), (19, 5), (19, 6), (19, 8),
(20, 2), (20, 3),
(21, 3), (21, 7), (21, 9),
(22, 2), (22, 3),
(23, 2), (23, 6), (23, 9),
(24, 2), (24, 3),
(25, 2), (25, 3),
(26, 2), (26, 3), (26, 8), (26, 9),
(27, 2), (27, 5),
(28, 2), (28, 3), (28, 6), (28, 9), (28, 4),
(29, 8), (29, 9),
(30, 8), (30, 9),
(31, 2), (31, 8),
(32, 2), (32, 3), (32, 10),(32, 9), (32, 11),
(33, 9), (33, 11),
(34, 5),
(35, 5),
(36, 2),
(37, 8), (37, 5), (37, 9),
(38, 8), (38, 5), (38, 9),
(39, 7), (39, 9), (39, 4),
(40, 2),
(41, 2), (41, 3), (41, 5),
(42, 2),
(43, 9), (43, 4),
(44, 2), (44, 3), (44, 8),
(45, 2), (45, 3), (45, 5),
(46, 2), (46, 3),
(47, 3), (47, 5), (47, 9),
(48, 3), (48, 7), (48, 8), (48, 9),
(49, 2), (49, 3), (49, 7),
(50, 2), (50, 3),
(51, 2),
(52, 2),
(53, 2),
(54, 2),
(55, 2),
(56, 3), (56, 5), (56, 4),
(57, 3), (57, 8), (57, 5), (57, 4),
(58, 3), (58, 5), (58, 4),
(59, 5),
(60, 5),
(61, 5),
(61, 2), (61, 8),
(62, 3), (62, 8),
(63, 8),
(64, 2), (64, 3),
(65, 2), (65, 3), (65, 8), (65, 5),
(66, 2), (66, 3), (66, 5),
(67, 2), (67, 8),
(68, 2), (68, 3), (68, 5),
(69, 2), (69, 3),
(70, 2), (70, 3), (70, 8),
(71, 4),
(72, 9), (72, 4),
(73, 2), (73, 3),
(74, 9), (74, 11), (74, 4),
(75, 9), (75, 11),
(76, 5),
(77, 2), (77, 3),
(78, 5), (78, 11),
(79, 2), (79, 3),
(80, 2), (80, 3),
(81, 2), (81, 10),
(82, 9), (82, 11),
(83, 2), (83, 7), (83, 4),
(84, 2), (84, 10), (84, 9), (84, 11),
(85, 2), (85, 3), (85, 7),
(86, 2), (86, 3), (86, 8),
(87, 7), (87, 9), (87, 11), (87, 4),
(88, 2), (88, 5),
(89, 3),
(90, 2), (90, 3),
(91, 2),
(92, 2), (92, 10),
(93, 2), (93, 3), (93, 7), (93, 10), (93, 11),
(94, 2),
(95, 2), (95, 9),
(96, 6), (96, 5),
(97, 2), (97, 11),
(98, 2), (98, 3), (98, 10),
(99, 2), (99, 3), (99, 8),
(100, 2);