/*
SQLyog Community v13.1.9 (64 bit)
MySQL - 5.6.51 : Database - Auth
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`Auth` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;

USE `Auth`;

/*Table structure for table `Accounts` */

DROP TABLE IF EXISTS `Accounts`;

CREATE TABLE `Accounts` (
  `AccountId` varchar(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` varchar(128) COLLATE utf8mb4_unicode_ci NOT NULL,
  `SessionId` char(64) COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Authority` varchar(512) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`AccountId`),
  UNIQUE KEY `SessionId` (`SessionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*Table structure for table `Worlds` */

DROP TABLE IF EXISTS `Worlds`;

CREATE TABLE `Worlds` (
  `WorldName` char(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `WorldType` char(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Address` varchar(128) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ExpireAt` datetime NOT NULL,
  PRIMARY KEY (`WorldName`),
  KEY `WorldType` (`WorldType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
