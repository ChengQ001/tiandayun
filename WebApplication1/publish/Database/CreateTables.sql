-- 创建数据库
CREATE DATABASE IF NOT EXISTS computer_inventory DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE computer_inventory;

-- 创建电脑主表
CREATE TABLE IF NOT EXISTS ComputerMain (
    Id INT AUTO_INCREMENT PRIMARY KEY COMMENT '电脑ID',
    ComputerName VARCHAR(255) NOT NULL COMMENT '电脑名称',
    CreateTime DATETIME NOT NULL COMMENT '电脑录入时间',
    SaleStatus TINYINT NOT NULL COMMENT '销售状态(1待安装2配件异常3待出售4已售)',
    StatusTime DATETIME NOT NULL COMMENT '状态时间',
    CostAmount DECIMAL(10, 2) NOT NULL COMMENT '电脑成本金额',
    SaleAmount DECIMAL(10, 2) NOT NULL DEFAULT 0.00 COMMENT '电脑售出金额',
    PlatformCommission DECIMAL(10, 2) NOT NULL DEFAULT 0.00 COMMENT '平台提成金额',
    ProfitAmount DECIMAL(10, 2) NOT NULL DEFAULT 0.00 COMMENT '最终利润金额',
    INDEX idx_CreateTime (CreateTime),
    INDEX idx_SaleStatus (SaleStatus)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='电脑信息主表';

-- 创建电脑配件详情表
CREATE TABLE IF NOT EXISTS ComputerPart (
    Id INT AUTO_INCREMENT PRIMARY KEY COMMENT '配件ID',
    ComputerId INT NOT NULL COMMENT '关联主表电脑ID',
    PartName VARCHAR(255) NOT NULL COMMENT '配件名称',
    PartType VARCHAR(100) NOT NULL COMMENT '配件类型',
    PartCost DECIMAL(10, 2) NOT NULL COMMENT '配件成本金额',
    PartStatus TINYINT NOT NULL COMMENT '配件状态(1正常2异常)',
    CreateTime DATETIME NOT NULL COMMENT '录入时间',
    ThirdPartyOrder VARCHAR(255) DEFAULT '' COMMENT '第三方平台订单信息',
    PurchasePlatform TINYINT NOT NULL DEFAULT 1 COMMENT '采购平台类型(1咸鱼)',
    FOREIGN KEY (ComputerId) REFERENCES ComputerMain(Id) ON DELETE CASCADE,
    INDEX idx_ComputerId (ComputerId),
    INDEX idx_PartType (PartType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='电脑配件详情表';

-- 插入示例数据（可选）
-- INSERT INTO ComputerMain (ComputerName, CreateTime, SaleStatus, StatusTime, CostAmount) VALUES ('测试电脑', NOW(), 1, NOW(), 2800.00);
-- INSERT INTO ComputerPart (ComputerId, PartName, PartType, PartCost, PartStatus, CreateTime) VALUES (1, '七彩虹h610m-e', '主板', 190.00, 1, NOW());
