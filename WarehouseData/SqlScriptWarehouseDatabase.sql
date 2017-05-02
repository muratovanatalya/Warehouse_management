/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Schema Warehouse
--
DROP SCHEMA IF EXISTS `Warehouse` ;
CREATE SCHEMA `Warehouse` DEFAULT CHARACTER SET utf8 ;
USE `Warehouse` ;

--
-- Table structure for table `Area`
--

DROP TABLE IF EXISTS `Area`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Area` (
  `Id` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Area`
--

LOCK TABLES `Area` WRITE;
/*!40000 ALTER TABLE `Area` DISABLE KEYS */;
INSERT INTO `Area` VALUES ('A'),('B'),('C'),('D'),('E'),('F'),('G'),('H'),('I'),('J'),('K');
/*!40000 ALTER TABLE `Area` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Hangar`
--

DROP TABLE IF EXISTS `Hangar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Hangar` (
  `Id` varchar(45) NOT NULL,
  `MaxContainers` int(11) NOT NULL,
  `PlacedContainers` int(11) NOT NULL,
  `AreaId` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`,`AreaId`),
  KEY `fk_Hangar_Area_idx` (`AreaId`),
  CONSTRAINT `fk_Hangar_Area` FOREIGN KEY (`AreaId`) REFERENCES `Area` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Hangar`
--

LOCK TABLES `Hangar` WRITE;
/*!40000 ALTER TABLE `Hangar` DISABLE KEYS */;
INSERT INTO `Hangar` VALUES ('A1',40,0,'A'),('A2',28,27,'A'),('A3',35,33,'A'),('A4',35,20,'A'),('A5',24,18,'A'),('A6',30,24,'A'),('B1',38,33,'B'),('B2',22,22,'B'),('B3',25,23,'B'),('B4',35,30,'B'),('B5',40,40,'B'),('B6',25,25,'B'),('B7',28,28,'B'),('B8',34,34,'B'),('C1',36,30,'C'),('C10',40,24,'C'),('C2',24,12,'C'),('C3',38,11,'C'),('C4',40,5,'C'),('C5',22,20,'C'),('C6',30,25,'C'),('C7',30,15,'C'),('C8',25,5,'C'),('C9',25,25,'C'),('D1',35,35,'D'),('D2',36,36,'D'),('D3',27,27,'D'),('D4',30,30,'D'),('D5',34,34,'D'),('D6',25,25,'D'),('E1',30,30,'E'),('E2',35,35,'E'),('E3',29,29,'E'),('E4',28,26,'E'),('E5',34,34,'E'),('E6',27,9,'E'),('E7',28,28,'E'),('F1',32,12,'F'),('F2',37,24,'F'),('F3',26,14,'F'),('F4',34,10,'F'),('F5',37,20,'F'),('F6',31,19,'F'),('F7',25,10,'F'),('F8',40,40,'F'),('F9',40,36,'F'),('G1',35,14,'G'),('G2',27,12,'G'),('G3',30,18,'G'),('G4',36,16,'G'),('G5',40,25,'G'),('G6',35,24,'G'),('G7',38,14,'G'),('G8',37,1,'G'),('H1',24,2,'H'),('H10',34,22,'H'),('H11',25,9,'H'),('H2',29,3,'H'),('H3',35,10,'H'),('H4',36,19,'H'),('H5',27,24,'H'),('H6',35,25,'H'),('H7',31,30,'H'),('H8',26,5,'H'),('H9',29,12,'H'),('I1',35,21,'I'),('I2',25,8,'I'),('I3',28,14,'I'),('I4',29,18,'I'),('I5',25,5,'I'),('I6',25,8,'I'),('I7',30,3,'I'),('J1',24,8,'J'),('J2',29,13,'J'),('J3',30,18,'J'),('J4',38,11,'J'),('J5',40,22,'J'),('J6',38,29,'J'),('J7',30,12,'J'),('J8',30,4,'J'),('K1',40,11,'K'),('K10',35,16,'K'),('K11',38,14,'K'),('K12',37,34,'K'),('K13',28,22,'K'),('K14',27,19,'K'),('K2',40,22,'K'),('K3',35,6,'K'),('K4',36,2,'K'),('K5',28,14,'K'),('K6',24,1,'K'),('K7',28,17,'K'),('K8',33,19,'K'),('K9',29,29,'K');
/*!40000 ALTER TABLE `Hangar` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `CheckNumberOfContainers` BEFORE INSERT ON Warehouse.Hangar FOR EACH ROW BEGIN
	IF (NEW.MaxContainers < 20 OR NEW.MaxContainers > 40) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Неверный ввод данных! Максимально возможное число контейнеров в ангаре должно быть в промежутке от 20 до 40.';
	END IF;
	IF (NEW.PlacedContainers > NEW.MaxContainers) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Неверный ввод данных! Число размещенных контейнеров в ангаре дожно быть не больше максимально возможного числа контейнеров.';
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `UpdateNumberOfContainers` BEFORE UPDATE ON Warehouse.Hangar FOR EACH ROW BEGIN
	IF (NEW.MaxContainers < 20 OR NEW.MaxContainers > 40) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Неверный ввод данных! Максимально возможное число контейнеров в ангаре должно быть в промежутке от 20 до 40.';
	END IF;
	IF (NEW.PlacedContainers > NEW.MaxContainers) THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Неверный ввод данных! Число размещенных контейнеров в ангаре дожно быть не больше максимально возможного числа контейнеров.';
	END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-05-02 15:52:02
