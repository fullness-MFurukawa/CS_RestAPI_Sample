# RestAPI_Sample


- EF Coreのインストール
```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql 

```
- サンプルテーブルの作成
```sql
-- 部署テーブル: department
CREATE TABLE `department` (
    `dept_id` int NOT NULL AUTO_INCREMENT,
    `dept_uuid` varchar(36) COLLATE utf8mb4_general_ci NOT NULL,
    `dept_name` varchar(30) COLLATE utf8mb4_general_ci NOT NULL,
    PRIMARY KEY (`dept_id`),
    UNIQUE KEY `idx_dept_uuid` (`dept_uuid`)
) ENGINE = InnoDB AUTO_INCREMENT = 4 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

-- 従業員テーブル: employee
CREATE TABLE `employee` (
    `emp_id` int NOT NULL AUTO_INCREMENT,
    `emp_uuid` varchar(36) COLLATE utf8mb4_general_ci NOT NULL,
    `emp_name` varchar(30) COLLATE utf8mb4_general_ci NOT NULL,
    `dept_id` int DEFAULT NULL,
    PRIMARY KEY (`emp_id`),
    UNIQUE KEY `idx_emp_uuid` (`emp_uuid`),
    KEY `dept_id` (`dept_id`),
    CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`dept_id`) REFERENCES `department` (`dept_id`)
) ENGINE = InnoDB AUTO_INCREMENT = 11 DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;

```

- MySQLの起動
```bash
sudo systemctl start mysql
```
- MySQLの起動確認
```bash
sudo systemctl status mysql

[sudo] password for ubuntu: 
● mysql.service - MySQL Community Server
     Loaded: loaded (/usr/lib/systemd/system/mysql.service; disabled; preset: ena>
     Active: active (running) since Sat 2025-07-05 12:57:52 JST; 18min ago
    Process: 2734 ExecStartPre=/usr/share/mysql/mysql-systemd-start pre (code=exi>
   Main PID: 2743 (mysqld)
     Status: "Server is operational"
      Tasks: 40 (limit: 9302)
     Memory: 438.1M (peak: 438.4M)
        CPU: 9.808s
     CGroup: /system.slice/mysql.service
             └─2743 /usr/sbin/mysqld

Jul 05 12:57:51 DESKTOP-F7R4MFG systemd[1]: Starting mysql.service - MySQL Commun>
Jul 05 12:57:52 DESKTOP-F7R4MFG systemd[1]: Started mysql.service - MySQL Communi>
lines 1-14/14 (END)
```

- Moqのインストール
```bash
cd Tests/RestAPI_Sample.Application.Tests
dotnet add package Moq
```

- Moqのインストール
```bash
cd Tests/RestAPI_Sample.Infrastructure.Tests
dotnet add package Moq
```

- Microsoft.Extensions.DependencyInjectionのインストール
```bash
dotnet add package Microsoft.Extensions.DependencyInjection
```

- Swashbuckle のアノテーション属性のインストール
```bash
dotnet add package Swashbuckle.AspNetCore.Annotations
または
dotnet add package Swashbuckle.AspNetCore --version 6.5.0

```
