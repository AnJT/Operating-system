

# Operating-system

[TOC]

## 进程管理-电梯调度

> 题目：电梯调度
>
> 指导教师：张慧娟
>
> 姓名：安江涛
>
> 学号：1952560

![](https://github.com/AnJT/IMG/blob/main/elev.png?raw=true)

### 方案

- #### 单独电梯调度算法——LOOK算法

  > LOOK算法是扫描算法的一种改进。扫描算法(SCAN)是一种按照楼层顺序依次服务请求的算法，它让电梯在最底层和最顶层之间连续往返运行，在运行过程中响应处在于电梯运行方向相同的各楼层上的请求。扫描算法较好地解决了电梯移动的问题，在这个算法中，每个电梯响应乘客请求使乘客获得服务的次序是由其发出请求的乘客的位置与当前电梯位置之间的距离来决定的，所有的与电梯运行方向相同的乘客的请求在一次电梯向上运行或向下运行的过程中完成，免去了电梯频繁的来回移动。

- ####  联合电梯调度算法

  > 综合电梯运行状态、消息队列计算dist（距离 + 2 * 停靠数），选取最小值，加入某部电梯的消息队列中。

### 开发工具

- ##### 开发环境

  Windows 10

  Pycharm

- ##### 开发语言：

  python3

### 运行

`pip install PyQt5`

`pip install pyqt5-tools`

`python main.py`