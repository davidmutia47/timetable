-- MySQL dump 10.13  Distrib 5.5.30, for Win32 (x86)
--
-- Host: localhost    Database: timetable
-- ------------------------------------------------------
-- Server version	5.5.30

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
-- Table structure for table `courses`
--

DROP TABLE IF EXISTS `courses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `courses` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `initials` varchar(20) NOT NULL,
  `faculty` varchar(20) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `initials` (`initials`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `courses`
--

LOCK TABLES `courses` WRITE;
/*!40000 ALTER TABLE `courses` DISABLE KEYS */;
INSERT INTO `courses` VALUES (1,'BMCS','C.I.T'),(2,'BCS','C.I.T'),(3,'BBIT','Business & Law'),(4,'BCOM','Business'),(5,'Media','FAMECO');
/*!40000 ALTER TABLE `courses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lecturers`
--

DROP TABLE IF EXISTS `lecturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `lecturers` (
  `id` int(15) NOT NULL,
  `name` varchar(30) NOT NULL,
  `faculty` varchar(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lecturers`
--

LOCK TABLES `lecturers` WRITE;
/*!40000 ALTER TABLE `lecturers` DISABLE KEYS */;
INSERT INTO `lecturers` VALUES (9870973,'Dr. Iyaya','Science'),(31245786,'Prof. Ngigi','Science'),(31351914,'Dr. Davis','C.I.T'),(321456879,'Dr. Karanja','C.I.T');
/*!40000 ALTER TABLE `lecturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rooms`
--

DROP TABLE IF EXISTS `rooms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rooms` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `number` varchar(12) NOT NULL,
  `location` varchar(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rooms`
--

LOCK TABLES `rooms` WRITE;
/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms` VALUES (1,'Eg32','It Block'),(2,'Eg30','Block B'),(3,'Pref-001','Preferbs'),(4,'Eng 302','Engineering Block');
/*!40000 ALTER TABLE `rooms` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `students` (
  `regno` varchar(20) NOT NULL,
  `name` varchar(50) NOT NULL,
  `course` int(11) NOT NULL,
  `email` varchar(30) NOT NULL,
  `phone` varchar(20) NOT NULL,
  PRIMARY KEY (`regno`),
  KEY `course` (`course`),
  CONSTRAINT `students_ibfk_1` FOREIGN KEY (`course`) REFERENCES `courses` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES ('BUS-223-026/2013','Nyambura Jane',3,'davidmutia47@gmail.com','2864567'),('BUS-98-98/6756','Ayumbu dablo',4,'cyprianmutia@gmail.com','234567898'),('CIT-224-011/2013','Daniel Mutuku',2,'danelmutuku@gmail.com','09763456'),('CIT-224-096/2013','Dave Mutia',1,'davidmutia47@gmail.com','2864567'),('CIT-342-009/2015','Kimondolo Kimani',1,'davidmutia47j@hotmail.com','345678'),('ENG-223-026/2013','Mutuku Daniel',2,'danielavedon12@gmail.com','2864567');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `units`
--

DROP TABLE IF EXISTS `units`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `units` (
  `code` varchar(10) NOT NULL,
  `name` varchar(30) NOT NULL,
  PRIMARY KEY (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `units`
--

LOCK TABLES `units` WRITE;
/*!40000 ALTER TABLE `units` DISABLE KEYS */;
INSERT INTO `units` VALUES ('ICS201','Advanced Database'),('SMA2019','Fluid Mechanics'),('SMA204','Regression Analysis');
/*!40000 ALTER TABLE `units` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unitsdonebycourse`
--

DROP TABLE IF EXISTS `unitsdonebycourse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unitsdonebycourse` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `unit` varchar(10) NOT NULL,
  `course` int(11) NOT NULL,
  `lecturer` int(15) NOT NULL,
  `room` int(11) NOT NULL,
  `day` int(1) NOT NULL,
  `timeFrom` time NOT NULL,
  `timeTo` time NOT NULL,
  PRIMARY KEY (`id`),
  KEY `unit` (`unit`),
  KEY `course` (`course`),
  KEY `lecturer` (`lecturer`),
  KEY `room` (`room`),
  CONSTRAINT `unitsdonebycourse_ibfk_1` FOREIGN KEY (`unit`) REFERENCES `units` (`code`),
  CONSTRAINT `unitsdonebycourse_ibfk_2` FOREIGN KEY (`course`) REFERENCES `courses` (`id`),
  CONSTRAINT `unitsdonebycourse_ibfk_3` FOREIGN KEY (`lecturer`) REFERENCES `lecturers` (`id`),
  CONSTRAINT `unitsdonebycourse_ibfk_4` FOREIGN KEY (`room`) REFERENCES `rooms` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unitsdonebycourse`
--

LOCK TABLES `unitsdonebycourse` WRITE;
/*!40000 ALTER TABLE `unitsdonebycourse` DISABLE KEYS */;
INSERT INTO `unitsdonebycourse` VALUES (1,'SMA204',1,9870973,1,1,'08:00:36','22:00:36'),(2,'ICS201',4,31351914,3,1,'11:00:36','13:00:36'),(3,'ICS201',2,321456879,1,5,'12:45:00','16:45:04'),(4,'SMA2019',3,31351914,4,3,'10:00:03','13:01:03'),(5,'SMA2019',5,9870973,3,6,'07:30:08','09:30:08'),(6,'SMA2019',1,31245786,2,6,'15:00:43','17:47:43'),(7,'SMA2019',2,9870973,3,2,'09:40:41','11:30:41'),(8,'SMA2019',2,9870973,3,2,'13:30:56','14:13:56');
/*!40000 ALTER TABLE `unitsdonebycourse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `pass` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1234,'e1f1ee0cfcbb93ea2ffe9e44df18395c');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-11-15 23:49:44
