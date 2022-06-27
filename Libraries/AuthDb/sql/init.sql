CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Accounts` (
    `AccountId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Token` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NOT NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`AccountId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Courses` (
    `CourseId` int NOT NULL,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Credits` int NOT NULL,
    CONSTRAINT `PK_Courses` PRIMARY KEY (`CourseId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Credentials` (
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Token` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Credentials` PRIMARY KEY (`Name`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Foos` (
    `Seq` bigint NOT NULL AUTO_INCREMENT,
    `AccountId` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Command` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Value` int NOT NULL,
    CONSTRAINT `PK_Foos` PRIMARY KEY (`Seq`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Maintenance` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Start` datetime(6) NOT NULL,
    `End` datetime(6) NOT NULL,
    `Reason` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Maintenance` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Servers` (
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NOT NULL,
    `Address` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ExpireAt` datetime(6) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Servers` PRIMARY KEY (`Name`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Students` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `LastName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `FirstMidName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `EnrollmentDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_Students` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Enrollments` (
    `EnrollmentId` int NOT NULL AUTO_INCREMENT,
    `CourseId` int NOT NULL,
    `StudentId` int NOT NULL,
    `Grade` int NULL,
    CONSTRAINT `PK_Enrollments` PRIMARY KEY (`EnrollmentId`),
    CONSTRAINT `FK_Enrollments_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`CourseId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Enrollments_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Enrollments_CourseId` ON `Enrollments` (`CourseId`);

CREATE INDEX `IX_Enrollments_StudentId` ON `Enrollments` (`StudentId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220624105609_InitialCreate', '6.0.6');

COMMIT;

