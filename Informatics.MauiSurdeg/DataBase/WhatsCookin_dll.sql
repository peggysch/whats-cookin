CREATE TABLE AppUser (
    AppUserID     INT IDENTITY(1,1) NOT NULL,
    UserNo        NVARCHAR(50) UNIQUE NOT NULL,
    Username      NVARCHAR(50) NOT NULL,
    PasswordHash  VARBINARY(64) NOT NULL,

    CONSTRAINT PK_AppUser_AppUserID PRIMARY KEY (AppUserID),
    CONSTRAINT UQ_AppUser_Username UNIQUE (Username),

    -- Password complexity check
    CONSTRAINT CK_AppUser_PasswordComplexity CHECK (LEN(PasswordHash) > 0)
);

CREATE TABLE Recipe (
    RecipeID     INT IDENTITY(1, 1) NOT NULL,
    RecipeNo     NVARCHAR(50) UNIQUE NOT NULL,
    Title        NVARCHAR(255) NOT NULL,
    RecipeDescription  NVARCHAR(MAX) NOT NULL,
    CookingTime  INT CHECK (CookingTime > 0) NOT NULL,
    CreatedAt    DATETIME DEFAULT GETDATE(),
    AppUserID    INT NOT NULL,

    CONSTRAINT PK_Recipe_RecipeID PRIMARY KEY (RecipeID),
    CONSTRAINT FK_Recipe_User FOREIGN KEY (AppUserID) REFERENCES AppUser(AppUserID) ON DELETE CASCADE
);

CREATE TABLE Ingredient (
    IngredientID     INT IDENTITY(1, 1) NOT NULL,
    IngredientNo     NVARCHAR(50) UNIQUE NOT NULL,
    IngredientName   NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_Ingredient_IngredientID PRIMARY KEY (IngredientID)
);

CREATE TABLE RecipeIngredient (
    RecipeID       INT NOT NULL,
    IngredientID   INT NOT NULL,
    Quantity       NVARCHAR(50) NOT NULL,

    CONSTRAINT PK_RecipeIngredient_RecipeID_IngredientID PRIMARY KEY (RecipeID, IngredientID),
    CONSTRAINT FK_RecipeIngredient_Recipe FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE
);

CREATE TABLE Rating (
    RatingID    INT IDENTITY(1,1) NOT NULL,
    RatingNo    NVARCHAR(50) UNIQUE NOT NULL,
    AppUserID   INT NOT NULL,
    RecipeID    INT NOT NULL,
    Stars       INT NOT NULL CHECK (Stars BETWEEN 1 AND 5),
    CreatedAt   DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_Rating_RatingID PRIMARY KEY (RatingID),
    CONSTRAINT FK_Rating_RecipeID FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE,
    CONSTRAINT UQ_Rating_User_Recipe UNIQUE (AppUserID, RecipeID)
);

CREATE TABLE Comment (
    CommentID   INT IDENTITY(1,1) NOT NULL,
    CommentNo   NVARCHAR(50) UNIQUE NOT NULL,
    AppUserID   INT NOT NULL,
    RecipeID    INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedAt   DATETIME DEFAULT GETDATE(),

    CONSTRAINT PK_Comment_CommentID PRIMARY KEY (CommentID),
    CONSTRAINT UQ_Comment_CommentNo UNIQUE (CommentNo),
    CONSTRAINT FK_Comment_Recipe FOREIGN KEY (RecipeID) REFERENCES Recipe(RecipeID) ON DELETE CASCADE
);

-- Insert Users with Hashed Passwords
INSERT INTO AppUser (UserNo, Username, PasswordHash)
VALUES
('U001', 'Pasta_papi', HASHBYTES('SHA2_256', 'password123')),
('U002', 'Mumsmamman', HASHBYTES('SHA2_256', 'mySecureP@ssw0rd')),
('U003', 'test123', HASHBYTES('SHA2_256', 'test123')),
('U004', 'Bobbe bollo', HASHBYTES('SHA2_256', 'bobthebuilder2025'));

-- Insert Recipes (linked to Users)
INSERT INTO Recipe (RecipeNo, Title, RecipeDescription, CookingTime, AppUserID)
VALUES
('R001', 'Spaghetti Bolognese', 'A classic Italian dish with ground beef, tomatoes, and spices.', 30, 1),
('R002', 'Chicken Curry', 'A spicy chicken curry with coconut milk and curry powder.', 45, 2),
('R003', 'Vegetable Stir Fry', 'A quick stir fry with seasonal vegetables and soy sauce.', 20, 3),
('R004', 'Beef Stew', 'Tender beef chunks slow-cooked with carrots and potatoes in a savory broth.', 120, 4);

-- Insert Ingredients
INSERT INTO Ingredient (IngredientNo, IngredientName)
VALUES
('I001', 'Ground Beef'),
('I002', 'Tomatoes'),
('I003', 'Spaghetti'),
('I004', 'Chicken Breast'),
('I005', 'Coconut Milk'),
('I006', 'Carrots'),
('I007', 'Soy Sauce'),
('I008', 'Potatoes'),
('I009', 'Garlic'),
('I010', 'Olive Oil');

-- Insert RecipeIngredients (Linking Recipes to Ingredients)
INSERT INTO RecipeIngredient (RecipeID, IngredientID, Quantity)
VALUES
(1, 1, '500g'),
(1, 2, '3 Tomatoes'),
(1, 3, '200g'),
(2, 4, '300g'),
(2, 5, '200ml'),
(3, 6, '2 Carrots'),
(3, 7, '50ml'),
(4, 1, '600g'),
(4, 6, '3 Carrots'),
(4, 8, '4 Potatoes');

-- Insert Ratings (Users rating recipes)
INSERT INTO Rating (RatingNo, AppUserID, RecipeID, Stars)
VALUES
('RA001', 1, 1, 5),
('RA002', 2, 1, 4),
('RA003', 3, 2, 3),
('RA004', 4, 3, 5);

-- Insert Comments (Users commenting on recipes)
INSERT INTO Comment (CommentNo, AppUserID, RecipeID, CommentText)
VALUES
('C001', 1, 1, 'This recipe is fantastic! A family favorite!'),
('C002', 2, 2, 'I love how spicy this is, perfect for a chilly day.'),
('C003', 3, 3, 'Quick and easy! My go-to recipe for busy days.'),
('C004', 4, 4, 'The beef stew was amazing, so tender and flavorful!');