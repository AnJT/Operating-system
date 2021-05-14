import asyncio
import os
import sys

from PyQt5.QtCore import QCoreApplication, QPropertyAnimation, QRect, Qt
from PyQt5.QtGui import QCursor
from PyQt5.QtWidgets import (QApplication, QDesktopWidget, QGraphicsView,
                             QGridLayout, QLabel, QLCDNumber, QMessageBox,
                             QPushButton, QWidget)

from dispatch import Dispatch

OPEN = 0  # 开门状态
CLOSE = 1  # 关门状态

STILL = 0  # 静止状态
RUNNING_UP = 1  # 上行状态
RUNNING_DOWN = 2  # 下行状态

PE = 0 # 有动画
NOPE = 1 # 无动画

GO_UP = 0  # 用户要上行
GO_DOWN = 1  # 用户要下行


class Ui(QWidget):

    def __init__(self):
        super().__init__()

        self.ctrl = Dispatch(self)

        self.elev_enabled = [True] * 5  # 电梯状态（可使用/禁用）
        self.elev_state = [STILL] * 5  # 电梯运行状态（上/下/静止）
        self.elev_floor = [1] * 5  # 电梯所在的楼层
        self.door_state = [CLOSE] * 5  # 电梯门状态（开/关）

        self.anim_state = [NOPE] * 5
        self.floor_label = []
        self.upbtn = []
        self.downbtn = []
        self.label = []
        self.elev_front = []  # 电梯
        self.elev_back = []
        self.elev_anim = []
        self.led = []  # LED灯
        self.warnbtn = []  # 报警器
        self.grid_layout_widget = []  # 楼层按键
        self.grid_layout = []
        self.openbtn = []  # 开关
        self.closebtn = []

        self.initUI() 

    def initUI(self):
        self.resize(1280, 720)
        self.center()
        style_file = os.getcwd() + '\style.qss'
        self.setStyleSheet(str(self.loadQss(style_file))) # 加载QSS
        self.setWindowFlag(Qt.FramelessWindowHint) # 设置无边框
        self.setWindowOpacity(0.9)  # 设置窗口透明度

        # 关闭和缩小
        self.left_close = QPushButton(self)  # 关闭按钮
        self.left_visit = QPushButton(self)  # 空白按钮
        self.left_mini = QPushButton(self)  # 最小化按钮
        self.left_close.setFixedSize(20, 20)  # 设置关闭按钮的大小
        self.left_visit.setFixedSize(20, 20)  # 设置按钮大小
        self.left_mini.setFixedSize(20, 20)  # 设置最小化按钮大小
        self.left_close.setStyleSheet(
            '''QPushButton{background:#F76677;border-radius:10px;}QPushButton:hover{background:red;}''')
        self.left_visit.setStyleSheet(
            '''QPushButton{background:#F7D674;border-radius:10px;}QPushButton:hover{background:yellow;}''')
        self.left_mini.setStyleSheet(
            '''QPushButton{background:#6DDF6D;border-radius:10px;}QPushButton:hover{background:green;}''')
        self.left_close.move(20, 20)
        self.left_visit.move(50, 20)
        self.left_mini.move(80, 20)
        self.left_close.clicked.connect(QCoreApplication.instance().quit)
        self.left_mini.clicked.connect(self.showMinimized)

        # 画电梯和电梯动画
        elev_pos = [30, 280, 530, 780, 1030]
        for i in range(len(elev_pos)):
            self.elev_back.append(QGraphicsView(self))
            self.elev_back[i].setGeometry(elev_pos[i], 490, 131, 161)
            self.elev_back[i].setStyleSheet("background-color:gray")
            self.elev_back[i].setObjectName("elev_back" + str(i))

            # 电梯门
            self.elev_front.append(QGraphicsView(self))
            self.elev_front[2 * i].setGeometry(elev_pos[i], 490, 64, 161)
            self.elev_front[2 * i].setStyleSheet("background-color:white")
            self.elev_front[2 * i].setObjectName("elev_front" + str(2 * i))
            self.elev_anim.append(QPropertyAnimation(
                self.elev_front[i * 2], b"geometry"))
            self.elev_anim[2 * i].setDuration(1000)
            self.elev_anim[2 *
                           i].setStartValue(QRect(elev_pos[i], 490, 64, 161))
            self.elev_anim[2 * i].setEndValue(QRect(elev_pos[i], 490, 8, 161))

            self.elev_front.append(QGraphicsView(self))
            self.elev_front[2 * i +
                            1].setGeometry(elev_pos[i] + 67, 490, 64, 161)
            self.elev_front[2 * i + 1].setStyleSheet("background-color:white")
            self.elev_front[2 * i +
                            1].setObjectName("elev_front" + str(2 * i + 1))
            self.elev_anim.append(QPropertyAnimation(
                self.elev_front[i * 2 + 1], b"geometry"))
            self.elev_anim[2 * i + 1].setDuration(1000)
            self.elev_anim[2 * i +
                           1].setStartValue(QRect(elev_pos[i] + 67, 490, 64, 161))
            self.elev_anim[2 * i +
                           1].setEndValue(QRect(elev_pos[i] + 123, 490, 8, 161))

        # 电梯文字
        label_pos = [75, 325, 575, 835, 1075]
        for i in range(len(label_pos)):
            self.label.append(QLabel('电梯' + str(i + 1), self))
            self.label[i].setGeometry(label_pos[i], 660, 51, 21)
            self.label[i].setObjectName("label" + str(i))
            self.label[i].setStyleSheet("color:white")

        # 电梯楼层数码管
        led_pos = [50, 300, 550, 800, 1050]
        for i in range(len(led_pos)):
            self.led.append(QLCDNumber(self))
            self.led[i].setGeometry(led_pos[i] + 20, 440, 51, 41)
            self.led[i].setDigitCount(2)
            self.led[i].setProperty("value", 1.0)
            self.led[i].setObjectName("led" + str(i))

        # 报警器
        warnbtn_pos = [190, 440, 690, 940, 1190]
        for i in range(len(warnbtn_pos)):
            self.warnbtn.append(QPushButton(self))
            self.warnbtn[i].setGeometry(warnbtn_pos[i] + 12, 640, 56, 31)
            self.warnbtn[i].setStyleSheet(
                "background-color:red;border-radius:10px")
            self.warnbtn[i].setObjectName("warnbtn" + str(i))
            self.warnbtn[i].setText("报警器")
            self.warnbtn[i].clicked.connect(self.warnClick)

        # 按键（1-20）
        grid_pos = [190, 440, 690, 940, 1190]
        for i in range(len(grid_pos)):
            self.grid_layout_widget.append(QWidget(self))
            self.grid_layout_widget[i].setGeometry(grid_pos[i], 140, 81, 451)
            self.grid_layout_widget[i].setObjectName(
                "grid_layout_widget" + str(i))
            self.grid_layout.append(QGridLayout(self.grid_layout_widget[i]))
            self.grid_layout[i].setObjectName("grid_layout" + str(i))

        floor_name = ['19', '20', '17', '18', '15', '16', '13', '14', '11', '12', '9', '10',
                      '7', '8', '5', '6', '3', '4', '1', '2']
        positions = [(i, j) for i in range(10) for j in range(2)]
        for i in range(len(grid_pos)):
            for position, name in zip(positions, floor_name):
                button = QPushButton(name)
                button.setStyleSheet("border-radius: 11px")
                button.setObjectName("floorbtn " + str(i) + ' ' + name)
                button.clicked.connect(self.floorbtnClick)
                self.grid_layout[i].addWidget(button, *position)

        # 上行按钮
        for i in range(20):
            self.upbtn.append(QPushButton(self))
            self.upbtn[i].setGeometry(230 + 90*(i%10), 60 if i<10 else 90, 24, 24)
            self.upbtn[i].setStyleSheet("QPushButton{border-image: url(resources/up.png)}"
                                        "QPushButton:hover{border-image: url(resources/up_hover.png)}"
                                        "QPushButton:pressed{border-image: url(resources/up_pressed.png)}")
            self.upbtn[i].setObjectName("upbtn " + str(i))
            self.upbtn[i].clicked.connect(self.outClick)

            # 下行按钮
            self.downbtn.append(QPushButton(self))
            self.downbtn[i].setGeometry(260 + 90*(i%10), 60 if i<10 else 90, 24, 24)
            self.downbtn[i].setStyleSheet("QPushButton{border-image: url(resources/down.png)}"
                                          "QPushButton:hover{border-image: url(resources/down_hover.png)}"
                                          "QPushButton:pressed{border-image: url(resources/down_pressed.png)}")
            self.downbtn[i].setObjectName("downbtn " + str(i))
            self.downbtn[i].clicked.connect(self.outClick)

            self.floor_label.append(
                QLabel("0"+str(i + 1) if i < 9 else str(i+1), self))
            self.floor_label[i].setGeometry(200 + 90*(i%10), 60 if i<10 else 90, 24, 24)
            self.floor_label[i].setObjectName("label" + str(i))
            self.floor_label[i].setAlignment(Qt.AlignCenter)
            self.floor_label[i].setStyleSheet("color:white")

        # 开关
        openbtn_pos = [190, 440, 690, 940, 1190]
        closebtn_pos = [240, 490, 740, 990, 1240]
        for i in range(len(openbtn_pos)):
            self.openbtn.append(QPushButton(self))
            self.openbtn[i].setGeometry(openbtn_pos[i] + 10, 590, 27, 27)
            self.openbtn[i].setObjectName("openbtn" + str(i))
            self.openbtn[i].setStyleSheet("QPushButton{background: black;color:#fff;border-style: outset;border-radius: 11px}"
                                          "QPushButton:hover{background-color: #49afcd;border-color: #5599ff}"
                                          "QPushButton:pressed{border-radius:11px;background-color:red}"
                                          )
            self.openbtn[i].setText("开")
            self.openbtn[i].clicked.connect(self.doorClick)
            self.closebtn.append(QPushButton(self))
            self.closebtn[i].setGeometry(closebtn_pos[i] - 7, 590, 27, 27)
            self.closebtn[i].setObjectName("closebtn" + str(i))
            self.closebtn[i].setStyleSheet("QPushButton{background: black;color:#fff;border-style: outset;border-radius: 11px}"
                                           "QPushButton:hover{background-color: #49afcd;border-color: #5599ff}"
                                           "QPushButton:pressed{border-radius: 11px;background-color:red}"
                                           )
            self.closebtn[i].setText("关")
            self.closebtn[i].clicked.connect(self.doorClick)

    # 警报器槽函数
    def warnClick(self):
        sender = self.sender()
        print(sender.objectName() + ' was pressed')
        idx = int(sender.objectName()[-1])
        self.warnbtn[idx].setStyleSheet("background-color:black")
        QMessageBox.information(
            self.warnbtn[idx], "警告", "第" + str(idx + 1) + "号电梯已损坏")
        self.ctrl.warnCtrl(idx)
        

    # 开关门槽函数
    def doorClick(self):
        sender = self.sender()
        print(sender.objectName() + ' was pressed')
        idx = int(sender.objectName()[-1])
        command = OPEN if sender.objectName()[0] == 'o' else CLOSE
        print(idx, command)
        self.ctrl.doorCtrl(idx, command)

    # 楼层按键槽函数
    def floorbtnClick(self):
        sender = self.sender()
        print(sender.objectName() + ' was pressed')
        idxs = [int(s) for s in sender.objectName().split() if s.isdigit()]
        elev_idx = idxs[0]
        floor_idx = idxs[1]
        print(elev_idx, floor_idx)
        sender.setStyleSheet("border-radius: 11px;background-color:red")
        sender.setEnabled(False)
        self.ctrl.elevMove(elev_idx, floor_idx)

    def outClick(self):
        sender = self.sender()
        print(sender.objectName() + ' was pressed')

        floor = [int(s) for s in sender.objectName().split() if s.isdigit()]
        floor = floor[0]
        print("floor: ", floor)
        btn = sender.objectName()
        if btn[0] == 'd':
            self.downbtn[floor].setStyleSheet(
                "QPushButton{border-image: url(resources/down_pressed.png)}")
            self.downbtn[floor].setEnabled(False)
            choice = GO_DOWN
        else:
            self.upbtn[floor].setStyleSheet(
                "QPushButton{border-image: url(resources/up_pressed.png)}")
            self.upbtn[floor].setEnabled(False)

            choice = GO_UP
        print(floor, choice)
        self.ctrl.outCtrl(floor + 1, choice)

    # 居中显示
    def center(self):
        qr = self.frameGeometry()
        cp = QDesktopWidget().availableGeometry().center()
        qr.moveCenter(cp)
        self.move(qr.topLeft())

    # 加载QSS
    def loadQss(self, f):
        file = open(f)
        lines = file.readlines()
        res = ''
        for line in lines:
            res += line
        return res

    # 解决无边框后窗口无法移动
    def mousePressEvent(self, event):
        if event.button()==Qt.LeftButton:
            self.m_flag=True
            self.m_Position=event.globalPos()-self.pos() #获取鼠标相对窗口的位置
            event.accept()
            self.setCursor(QCursor(Qt.OpenHandCursor))  #更改鼠标图标
            
    def mouseMoveEvent(self, QMouseEvent):
        if Qt.LeftButton and self.m_flag:  
            self.move(QMouseEvent.globalPos()-self.m_Position)#更改窗口位置
            QMouseEvent.accept()
            
    def mouseReleaseEvent(self, QMouseEvent):
        self.m_flag=False
        self.setCursor(QCursor(Qt.ArrowCursor))


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ui = Ui()
    ui.show()
    sys.exit(app.exec_())
