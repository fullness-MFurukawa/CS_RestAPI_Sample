-- データベース作成
CREATE DATABASE restapi_sample CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE restapi_sample;

-- テーブル: department
CREATE TABLE department (
    dept_id    INT         NOT NULL AUTO_INCREMENT,
    dept_uuid  CHAR(36)    NOT NULL UNIQUE DEFAULT (UUID()),
    dept_name  VARCHAR(20) NOT NULL,
    PRIMARY KEY (dept_id)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;

-- テーブル: employee
CREATE TABLE employee (
    emp_id     INT         NOT NULL AUTO_INCREMENT,
    emp_uuid   CHAR(36)    NOT NULL UNIQUE DEFAULT (UUID()),
    emp_name   VARCHAR(20) NOT NULL,
    dept_id    INT         DEFAULT NULL,
    PRIMARY KEY (emp_id),
    KEY dept_id (dept_id),
    CONSTRAINT fk_employee_department FOREIGN KEY (dept_id)
        REFERENCES department (dept_id)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_unicode_ci;

-- 初期データ: department
INSERT INTO department (dept_name) VALUES
('営業部'),
('開発部'),
('人事部');

-- 初期データ: employee（10名）
INSERT INTO employee (emp_name, dept_id) VALUES
('田中太郎', 1),
('鈴木花子', 2),
('佐藤次郎', 1),
('伊藤美咲', 2),
('山田健太', 3),
('高橋真由', 2),
('渡辺一郎', 1),
('中村愛', 3),
('小林大輔', 2),
('加藤優子', 1);
このまま .sql ファイルとして