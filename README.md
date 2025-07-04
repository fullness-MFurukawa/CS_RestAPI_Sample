# RestAPI_Sample


- EF Coreのインストール
```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql 

```
- サンプルテーブルの作成
```sql
-- 部署テーブル: department
CREATE TABLE `department` (
    `dept_id` INT NOT NULL AUTO_INCREMENT,               -- 主キー（自動採番）
    `dept_uuid` CHAR(36) NOT NULL,                       -- UUID（ユニーク制約）
    `dept_name` VARCHAR(20) NOT NULL,                    -- 部署名
    PRIMARY KEY (`dept_id`),                             -- 主キーを dept_id に設定
    UNIQUE KEY `uk_dept_uuid` (`dept_uuid`)              -- UUIDにユニーク制約
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 従業員テーブル: employee
CREATE TABLE `employee` (
    `emp_id` INT NOT NULL AUTO_INCREMENT,                -- 主キー（自動採番）
    `emp_uuid` CHAR(36) NOT NULL,                        -- UUID（ユニーク制約）
    `emp_name` VARCHAR(20) NOT NULL,                     -- 従業員名
    `dept_uuid` CHAR(36),                                -- 外部キー: 所属部署UUID（NULL許容）
    PRIMARY KEY (`emp_id`),
    UNIQUE KEY `uk_emp_uuid` (`emp_uuid`),
    KEY `idx_dept_uuid` (`dept_uuid`),
    CONSTRAINT `fk_employee_department`
        FOREIGN KEY (`dept_uuid`)
        REFERENCES `department` (`dept_uuid`)
        ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

```
