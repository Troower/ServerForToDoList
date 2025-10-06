/*!40101 SET NAMES utf8mb4 */;

CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `last_name` varchar(50) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `surname` varchar(50) DEFAULT NULL,
  `login` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role` varchar(20) NOT NULL,
  `created_by` int DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `deleted_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `login` (`login`),
  KEY `created_by` (`created_by`),
  KEY `idx_users_deleted_at` (`deleted_at`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`created_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `users` VALUES (1,'Алексеевич','Вадим','Мартыненко','admin','$2a$10$cxpoKELU/O1SSTHboZefiueRYqiqzgYIQQLT7HuNNpHra9XyFQ3/S','admin',NULL,'2025-10-06 15:00:00',NULL);

DROP TABLE IF EXISTS `task_types`;

CREATE TABLE `task_types` (
  `type_id` int NOT NULL AUTO_INCREMENT,
  `type_name` varchar(50) NOT NULL,
  `is_accessible` tinyint DEFAULT '1',
  PRIMARY KEY (`type_id`),
  UNIQUE KEY `type_name` (`type_name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `task_types` VALUES (1,'Разработка',1),(2,'Тестирование',1),(3,'Дизайн',1),(4,'Документация',1),(5,'Администрирование',1);

DROP TABLE IF EXISTS `Tasks`;

CREATE TABLE `tasks` (
  `task_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(100) NOT NULL,
  `description` text,
  `due_date` datetime NOT NULL,
  `due_time` time DEFAULT NULL,
  `start_date` datetime DEFAULT NULL,
  `is_important` tinyint(1) DEFAULT '0',
  `type_id` int DEFAULT NULL,
  `status` tinyint(1) DEFAULT '0',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` int NOT NULL,
  `completed_at` datetime DEFAULT NULL,
  `is_confirmed` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`task_id`),
  KEY `type_id` (`type_id`),
  KEY `idx_tasks_due_date` (`due_date`),
  KEY `idx_tasks_status` (`status`),
  KEY `idx_tasks_created_by` (`created_by`),
  KEY `idx_tasks_is_confirmed` (`is_confirmed`),
  CONSTRAINT `tasks_ibfk_1` FOREIGN KEY (`type_id`) REFERENCES `task_types` (`type_id`),
  CONSTRAINT `tasks_ibfk_2` FOREIGN KEY (`created_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=75 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `task_assignments`;

CREATE TABLE `task_assignments` (
  `assignment_id` int NOT NULL AUTO_INCREMENT,
  `task_id` int NOT NULL,
  `user_id` int NOT NULL,
  `assigned_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `assigned_by` int DEFAULT NULL,
  PRIMARY KEY (`assignment_id`),
  UNIQUE KEY `task_id` (`task_id`,`user_id`),
  KEY `assigned_by` (`assigned_by`),
  KEY `idx_task_assignments_user_id` (`user_id`),
  CONSTRAINT `task_assignments_ibfk_1` FOREIGN KEY (`task_id`) REFERENCES `tasks` (`task_id`) ON DELETE CASCADE,
  CONSTRAINT `task_assignments_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`),
  CONSTRAINT `task_assignments_ibfk_3` FOREIGN KEY (`assigned_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `User_device_tokens`;

CREATE TABLE `user_device_tokens` (
  `token_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `device_token` varchar(255) NOT NULL,
  `device_type` varchar(50) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`token_id`),
  UNIQUE KEY `device_token` (`device_token`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `user_device_tokens_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
