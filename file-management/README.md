# 文件系统管理 项目文档

> 题目：文件系统管理
>
> 指导教师：张慧娟
>
> 姓名：安江涛
>
> 学号：1952560

## 一、项目概述

本项目为一个文件系统管理程序，在内存中开辟了1000B大小的空间作为文件存储器，并分为500个盘块，每一个盘块的大小为2B，退出本程序时，会将文件系统的内容保存到磁盘上，下次打开会将内容恢复到内存中。本项目文件存储空间管理采取的是链接结构，空闲空间管理采用位图，文件目录采用多级目录结构。

## 二、功能说明

本项目实现了如下功能：

- 格式化
- 创建子目录
- 删除子目录
- 显示目录
- 更改当前目录
- 创建文件
- 打开文件
- 关闭文件
- 写文件
- 读文件
- 重命名
- 删除文件

![](https://github.com/AnJT/IMG/blob/main/%E6%96%87%E4%BB%B61.png?raw=true)

![](https://github.com/AnJT/IMG/blob/main/%E6%96%87%E4%BB%B64.png?raw=true)

![](https://github.com/AnJT/IMG/blob/main/%E6%96%87%E4%BB%B63.png?raw=true)

## 三、项目实现

- #### 显示链接法

  本文件系统中，文件存储空间管理使用显示链接的方法，文件的内容存放在磁盘不同的块中，每次创建文件时为文件分配数量合适的空闲块；每次写文件时按顺序将文件内容写在相应块中；删除文件时将原先由内容的位置置为空即可。

- #### 位图、FAT表

  磁盘空闲空间广利在位图的基础上进行改造，将存放磁盘上文件位置信息的FAT表与传统的位图进行结合，磁盘空闲的位置使用`EMPTY = -1`标识，放有文件的盘块存放文件所在的下一个盘块的文职，文件存放结束的盘块位置使用`END = -2`标识。
  
- #### 基本功能流程图

  - 创建文件

    <img src="https://github.com/AnJT/IMG/blob/main/%E6%B5%811.png?raw=true" width=300px>

  - 修改文件内容

    <img src="https://github.com/AnJT/IMG/blob/main/%E6%B5%812.png?raw=true" width=400>

  - 删除文件

    <img src="https://github.com/AnJT/IMG/blob/main/%E6%B5%813.png?raw=true" width=250>

## 四、项目环境

- ##### 开发环境

  Windows 10

  Visual Studio 2022 Preview

  c#

- ##### 运行

  双击`exe`目录下的`FileManageSystem.exe`