# 电脑装配出售库存系统开发计划

## 一、项目结构规划

根据用户要求，项目将按照以下结构组织：

1. **Controllers/**: API接口层，处理HTTP请求
2. **Bll/**: 业务逻辑层，处理核心业务逻辑
3. **Services/**: 数据访问层，使用Dapper与数据库交互
4. **Tool/**: 工具类，包含DapperHelper等公共工具
5. **Models/**: 实体模型，定义数据结构

## 二、数据库设计

### 1. 数据库信息
   - 数据库名称：`computer_inventory`
   - 数据库类型：MySQL
   - 连接地址：10.100.21.61:3306
   - 用户名：root
   - 密码：Jnpf@2021

### 2. 电脑信息主表（ComputerMain）
| 字段名 | 数据类型 | 描述 |
| --- | --- | --- |
| Id | int | 主键，自增 |
| ComputerName | varchar(255) | 电脑名称 |
| CreateTime | datetime | 录入时间 |
| SaleStatus | int | 销售状态(1待安装2配件异常3待出售4已售) |
| StatusTime | datetime | 状态时间 |
| CostAmount | decimal(10,2) | 电脑成本金额 |
| SaleAmount | decimal(10,2) | 电脑售出金额 |
| PlatformCommission | decimal(10,2) | 平台提成金额 |
| ProfitAmount | decimal(10,2) | 最终利润金额 |

### 3. 电脑配件详情表（ComputerPart）
| 字段名 | 数据类型 | 描述 |
| --- | --- | --- |
| Id | int | 主键，自增 |
| ComputerId | int | 外键，关联主表Id |
| PartName | varchar(255) | 配件名称 |
| PartType | varchar(50) | 配件类型 |
| PartCost | decimal(10,2) | 配件成本金额 |
| PartStatus | int | 配件状态(1正常2异常) |
| CreateTime | datetime | 录入时间 |
| ThirdPartyOrder | varchar(255) | 第三方平台订单信息 |
| PurchasePlatform | int | 采购平台类型(1咸鱼) |

## 三、代码实现

### 1. 工具类（Tool/）
- `DapperHelper.cs`: 封装Dapper数据库操作，提供统一的数据访问接口

### 2. 实体模型（Models/）
- `ComputerMain.cs`: 电脑主表实体
- `ComputerPart.cs`: 电脑配件表实体
- `ComputerInputModel.cs`: 电脑信息录入模型，用于接收前端输入

### 3. 数据访问层（Services/）
- `ComputerService.cs`: 处理电脑主表的数据访问
- `ComputerPartService.cs`: 处理电脑配件表的数据访问

### 4. 业务逻辑层（Bll/）
- `ComputerBll.cs`: 处理电脑信息的业务逻辑，包括：
  - 解析前端输入的配件信息
  - 计算电脑总成本
  - 保存电脑信息（主表+配件表）
  - 获取电脑列表

### 5. API接口层（Controllers/）
- `ComputerController.cs`: 提供电脑信息的API接口，包括：
  - `Index()`: 返回电脑列表页面
  - `Create()`: 返回电脑录入页面
  - `Save()`: 接收并处理电脑录入数据
  - `GetComputerList()`: 返回电脑列表JSON数据

### 6. 视图层（Views/）
- `Computer/Index.cshtml`: 电脑列表页面，使用jQuery展示数据
- `Computer/Create.cshtml`: 电脑录入页面，包含文本域和提交按钮

## 四、核心功能实现

### 1. 电脑信息录入
- 前端提供文本域，用户按照指定格式输入配件信息
- 控制器接收输入，调用Bll层解析数据
- Bll层解析配件信息，计算总成本
- 调用Services层保存数据到数据库（主表+配件表）

### 2. 电脑列表展示
- 控制器调用Bll层获取电脑列表
- 前端使用jQuery渲染列表，展示销售状态、成本等信息

## 五、开发步骤

1. **准备工作**
   - 安装Dapper和MySql.Data依赖
   - 创建Bll和Models文件夹
   - 在Web.config中配置数据库连接字符串

2. **数据库层实现**
   - 创建数据库`computer_inventory`
   - 创建电脑主表和配件表

3. **工具类实现**
   - 在Tool文件夹中创建DapperHelper.cs

4. **实体模型实现**
   - 创建ComputerMain.cs和ComputerPart.cs实体类
   - 创建ComputerInputModel.cs用于接收前端输入

5. **数据访问层实现**
   - 创建ComputerService.cs和ComputerPartService.cs
   - 实现增删改查方法

6. **业务逻辑层实现**
   - 创建ComputerBll.cs
   - 实现配件信息解析逻辑
   - 实现成本计算逻辑
   - 实现数据保存逻辑

7. **API接口实现**
   - 创建ComputerController.cs
   - 实现页面路由和API接口

8. **前端页面实现**
   - 创建电脑录入页面Create.cshtml
   - 创建电脑列表页面Index.cshtml
   - 使用jQuery实现前端交互

9. **测试功能**
   - 测试电脑信息录入功能
   - 测试电脑列表展示功能

## 六、技术要点

- **Dapper**：用于高效数据库操作，封装在Tool/DapperHelper.cs中
- **jQuery**：用于前端交互和数据展示
- **分层架构**：严格按照Controllers、Bll、Services、Tool分层，实现关注点分离
- **事务处理**：保存电脑信息时使用事务，确保主表和配件表数据一致性
- **数据解析**：实现文本解析逻辑，将用户输入的配件信息转换为结构化数据

该计划将确保系统按照用户要求的结构组织代码，同时实现所需的功能。
