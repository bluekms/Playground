START TRANSACTION;

ALTER TABLE `Students` MODIFY COLUMN `LastName` varchar(50) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Students` MODIFY COLUMN `FirstMidName` varchar(50) CHARACTER SET utf8mb4 NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220627084651_MaxLengthOnNames', '6.0.6');

COMMIT;

