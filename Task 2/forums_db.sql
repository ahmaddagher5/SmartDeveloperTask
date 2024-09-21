-- First, let us define database entities:
-- 1- Forums
-- 2- Forum Categories
-- 3- Forum Posts
-- 4- Post Comments
-- 5- Tags
-- 6- Post Tags
-- 7- PrivateMessages
-- 8- Users
-- 9- UserProfiles

-- Therefore, Entities Relationships:

--  Categories -> Forums: One-to-many
--  Forums -> Posts: One-to-many
--  Posts -> Comments: One-to-many
--  Posts -> Tags: Many-to-many
--  Users -> Posts: One-to-many
--  Users -> Comments: One-to-many
--  Users -> PrivateMessages: One-to-many
--  Users -> UserProfiles: One-to-One



-- Second, now we will define tables:

-- Forums Table
CREATE TABLE Forum (
    ForumID INT PRIMARY KEY AUTO_INCREMENT,
    ForumCategoryID INT NOT NULL,
    ForumName VARCHAR(255) NOT NULL,
    ForumDescription TEXT,
    FOREIGN KEY (ForumCategoryID) REFERENCES ForumCategory(ForumCategoryID) ON DELETE CASCADE
);

-- Forum Categories Table
CREATE TABLE ForumCategory (
    ForumCategoryID INT PRIMARY KEY AUTO_INCREMENT,
    ForumCategoryName VARCHAR(255) NOT NULL
);

-- Posts Table
CREATE TABLE Post (
    PostID INT PRIMARY KEY AUTO_INCREMENT,
    ForumID INT NOT NULL,
    UserID INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Body TEXT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (ForumID) REFERENCES Forum(ForumID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES User(UserID) ON DELETE CASCADE
);

-- Post Comments Table
CREATE TABLE PostComment (
    PostCommentID INT PRIMARY KEY AUTO_INCREMENT,
    PostID INT NOT NULL,
    UserID INT NOT NULL,
    Body TEXT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (PostID) REFERENCES Post(PostID) ON DELETE CASCADE,
    FOREIGN KEY (UserID) REFERENCES User(UserID) ON DELETE CASCADE
);

-- Tags Master Table
CREATE TABLE Tag (
    TagID INT PRIMARY KEY AUTO_INCREMENT,
    TagName VARCHAR(100) NOT NULL
);

-- Post Tags Table
CREATE TABLE PostTag (
    PostID INT NOT NULL,
    TagID INT NOT NULL,
    PRIMARY KEY (PostID, TagID),
    FOREIGN KEY (PostID) REFERENCES Post(PostID) ON DELETE CASCADE,
    FOREIGN KEY (TagID) REFERENCES Tag(TagID) ON DELETE CASCADE
);
-- Private Messaging Table
CREATE TABLE PrivateMessage (
    MessageID INT PRIMARY KEY AUTO_INCREMENT,
    SenderID INT NOT NULL,
    ReceiverID INT NOT NULL,
    MessageBody TEXT NOT NULL,
    SentAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (SenderID) REFERENCES User(UserID) ON DELETE CASCADE,
    FOREIGN KEY (ReceiverID) REFERENCES User(UserID) ON DELETE CASCADE
);
-- Users Table
CREATE TABLE User (
    UserID INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- User Profiles Table
CREATE TABLE UserProfile (
    UserProfileID INT PRIMARY KEY AUTO_INCREMENT,
    UserID INT NOT NULL,
    Bio TEXT,
    AvatarURL VARCHAR(255),
    FOREIGN KEY (UserID) REFERENCES User(UserID) ON DELETE CASCADE
);

-- Third, Indexing for full-text search
CREATE FULLTEXT INDEX idx_post_body ON Post(Title, Body);
CREATE FULLTEXT INDEX idx_comment_body ON Comment(Body);

-- Fourth, Indexing for efficient query
CREATE INDEX idx_forumid ON Post(ForumID);
CREATE INDEX idx_postid ON Comment(PostID);
CREATE INDEX idx_userid_post ON Post(UserID);
CREATE INDEX idx_userid_comment ON Comment(UserID);

-- Fifth, Denormalization Examples
ALTER TABLE Forum ADD COLUMN PostsCount INT DEFAULT 0;
ALTER TABLE Post ADD COLUMN CommentsCount INT DEFAULT 0;
ALTER TABLE User ADD COLUMN PostsCount INT DEFAULT 0;
ALTER TABLE User ADD COLUMN CommentsCount INT DEFAULT 0;

-- Finally, Partitioning Examples

-- Partitioning Posts by years (Range Partitioning)
ALTER TABLE Post
PARTITION BY RANGE (YEAR(CreatedAt)) (
    PARTITION p2022 VALUES LESS THAN (2023),
    PARTITION p2023 VALUES LESS THAN (2024),
    PARTITION p2024 VALUES LESS THAN (2025),
    PARTITION pmax VALUES LESS THAN MAXVALUE
);

-- Partitioning Posts by Forum (List Partitioning)
ALTER TABLE Post 
PARTITION BY LIST (ForumID) (
    PARTITION tech_posts VALUES IN (1),
    PARTITION sports_posts VALUES IN (2),
    PARTITION science_posts VALUES IN (3),
    PARTITION others_posts VALUES IN (4, 5, 6)
);

-- The End