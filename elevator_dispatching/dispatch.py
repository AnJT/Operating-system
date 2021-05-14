import time

import numpy as np
from PyQt5.QtCore import QAbstractAnimation, QTimer
from PyQt5.QtWidgets import QMessageBox, QPushButton

OPEN = 0  # 开门状态
CLOSE = 1  # 关门状态

STILL = 0  # 静止状态
RUNNING_UP = 1  # 上行状态
RUNNING_DOWN = 2  # 下行状态

PE = 0 # 有动画
NOPE = 1 # 无动画

GO_UP = 0  # 用户要上行
GO_DOWN = 1  # 用户要下行

INF = 200

class Dispatch(object):
    def __init__(self, Elev):
        self.elev = Elev

        # 每秒更新电梯状态
        self.timer1 = QTimer()
        self.timer1.timeout.connect(self.updateElevState)
        self.timer1.start(1000)
        self.state = [RUNNING_UP] * 5
        self.messages = []
        self.messages_other = []
        self.messages_reverse = []
        for i in range(5):
            self.messages.append([])
            self.messages_reverse.append([])
            self.messages_other.append([])

    def updateElevState(self):
        for i in range(len(self.messages)):
            
            if self.elev.anim_state[i] == PE:
                self.elev.anim_state[i] = NOPE
                continue

            if len(self.messages[i]):

                if self.elev.elev_state[i] == STILL:
                    if self.elev.door_state[i] != OPEN:
                        self.elev.door_state[i] = OPEN
                        self.openAnim(i)
                        self.elev.anim_state[i] = PE

                    if self.elev.elev_floor[i] < self.messages[i][0]:
                        self.elev.elev_state[i] = RUNNING_UP
                        if self.elev.door_state[i] != CLOSE:
                            self.elev.door_state[i] = CLOSE
                            self.closeAnim(i)
                            self.elev.anim_state[i] = PE
                    elif self.elev.elev_floor[i] > self.messages[i][0]: 
                        self.elev.elev_state[i] = RUNNING_DOWN
                        if self.elev.door_state[i] != CLOSE:
                            self.elev.door_state[i] = CLOSE
                            self.closeAnim(i)
                            self.elev.anim_state[i] = PE

                else:
                    floor = self.messages[i][0]
                    if self.elev.elev_floor[i] < floor: # 向上运动
                        self.elev.elev_state[i] = RUNNING_UP
                        
                        self.elev.elev_floor[i] += 1
                        self.elev.led[i].setProperty("value", self.elev.elev_floor[i])
                    
                    elif self.elev.elev_floor[i] > floor:
                        self.elev.elev_state[i] = RUNNING_DOWN
                        self.elev.elev_floor[i] -= 1
                        self.elev.led[i].setProperty("value", self.elev.elev_floor[i])
                    
                    else: # 电梯到目的地
                        self.openAnim(i)
                        self.elev.anim_state[i] = PE
                        self.elev.door_state[i] = OPEN
                        self.elev.elev_state[i] = STILL
                        self.messages[i].pop(0)
                        floorbtn = self.elev.findChild(QPushButton,
                        "floorbtn {} {}".format(i, self.elev.elev_floor[i]))
                        floorbtn.setStyleSheet("border-radius: 11px")
                        floorbtn.setEnabled(True)

                        if self.state[i] == RUNNING_DOWN:
                            outbtn = self.elev.findChild(QPushButton,
                            "downbtn {}".format(self.elev.elev_floor[i] - 1))
                            outbtn.setStyleSheet("QPushButton{border-image: url(resources/down.png)}"
                                          "QPushButton:hover{border-image: url(resources/down_hover.png)}"
                                          "QPushButton:pressed{border-image: url(resources/down_pressed.png)}")
                            outbtn.setEnabled(True)
                            print("downbtn {}".format(self.elev.elev_floor[i] - 1))
                        else:
                            outbtn = self.elev.findChild(QPushButton,
                            "upbtn {}".format(self.elev.elev_floor[i] - 1))
                            outbtn.setStyleSheet("QPushButton{border-image: url(resources/up.png)}"
                                        "QPushButton:hover{border-image: url(resources/up_hover.png)}"
                                        "QPushButton:pressed{border-image: url(resources/up_pressed.png)}")
                            outbtn.setEnabled(True)
                            print("upbtn {}".format(self.elev.elev_floor[i] - 1))

            elif len(self.messages_reverse[i]):
                self.state[i] = RUNNING_DOWN if self.state[i] == RUNNING_UP else RUNNING_UP
                self.messages[i] = self.messages_reverse[i].copy()
                self.messages_reverse[i].clear()
                self.messages_reverse[i] = self.messages_other[i].copy()
                self.messages_other[i].clear()

                self.messages[i] = list(set(self.messages[i]))
                self.messages[i].sort()
                if self.state[i] == RUNNING_DOWN:
                    self.messages[i].reverse()

            else:
                if self.elev.door_state[i] != CLOSE:
                    self.closeAnim(i)
                    self.elev.anim_state[i] = PE
                    self.elev.door_state[i] = CLOSE

    def outCtrl(self, floor, command):
        enabled_list = []
        for i in range(5):
            if self.elev.elev_enabled[i]:
                enabled_list.append(i)
        
        dist = [INF] * 5
        for idx in enabled_list:
            if self.elev.elev_state[idx] == RUNNING_UP and command ==GO_UP and floor > self.elev.elev_floor[idx]:
                dist[idx] = floor - self.elev.elev_floor[idx]
            elif self.elev.elev_state[idx] == RUNNING_DOWN and command ==GO_DOWN and floor < self.elev.elev_floor[idx]:
                dist[idx] = self.elev.elev_floor[idx] - floor
            elif self.elev.elev_state[idx] == STILL:
                dist[idx] = abs(self.elev.elev_floor[idx] - floor)
        print(dist)
        best_idx = dist.index(min(dist))
        if dist[best_idx] == 0:
            if self.elev.door_state[best_idx] != OPEN:
                self.elev.door_state[best_idx] = OPEN
                self.openAnim(best_idx)
                self.elev.anim_state[best_idx] = PE
            if command == RUNNING_DOWN:
                outbtn = self.elev.findChild(QPushButton,
                "downbtn {}".format(floor - 1))
                outbtn.setStyleSheet("QPushButton{border-image: url(resources/down.png)}"
                                "QPushButton:hover{border-image: url(resources/down_hover.png)}"
                                "QPushButton:pressed{border-image: url(resources/down_pressed.png)}")
                outbtn.setEnabled(True)
                print("downbtn {}".format(floor - 1))
            else:
                outbtn = self.elev.findChild(QPushButton,
                "upbtn {}".format(floor - 1))
                outbtn.setStyleSheet("QPushButton{border-image: url(resources/up.png)}"
                                        "QPushButton:hover{border-image: url(resources/up_hover.png)}"
                                        "QPushButton:pressed{border-image: url(resources/up_pressed.png)}")
                outbtn.setEnabled(True)
                print("upbtn {}".format(floor - 1))
        else:
            self.messages[best_idx].append(floor)
        print(self.messages)

            

    def elevMove(self, elev_idx, floor_idx):
        now_floor = self.elev.elev_floor[elev_idx]

        if now_floor < floor_idx: # 当前层小于按键
            if self.state[elev_idx] == RUNNING_UP:
                self.messages[elev_idx].append(floor_idx)
                self.messages[elev_idx] = list(set(self.messages[elev_idx]))
                self.messages[elev_idx].sort()
            else:
                self.messages_reverse[elev_idx].append(floor_idx)
            
        elif now_floor > floor_idx: # 当前层大于按键
            if self.state[elev_idx] == RUNNING_UP:
                self.messages_reverse[elev_idx].append(floor_idx)
            else:
                self.messages[elev_idx].append(floor_idx)
                self.messages[elev_idx] = list(set(self.messages[elev_idx]))
                self.messages[elev_idx].sort()
                self.messages[elev_idx].reverse()
        
        else:
            if self.elev.elev_state[elev_idx] == STILL:
                if self.elev.door_state[elev_idx] != OPEN:
                    self.elev.door_state[elev_idx] = OPEN
                    self.openAnim(elev_idx)
                    self.elev.anim_state[elev_idx] = PE
            
            floorbtn = self.elev.findChild(QPushButton,
                "floorbtn {} {}".format(elev_idx, now_floor))
            floorbtn.setStyleSheet("border-radius: 11px")
            floorbtn.setEnabled(True)

    def warnCtrl(self, idx):
        self.elev.elev_enabled[idx] = False # 禁用电梯
        self.elev.elev_back[idx].setEnabled(False) # 禁用电梯背景
        self.elev.elev_front[2 * idx].setEnabled(False) #禁言电梯门
        self.elev.elev_front[2 * idx + 1].setEnabled(False)
        self.elev.elev_anim[2 * idx].stop() # 停止电梯动画
        self.elev.elev_anim[2 * idx + 1].stop()
        self.elev.led[idx].setEnabled(False) # 禁用LED显示灯
        self.elev.warnbtn[idx].setEnabled(False) # 禁用报警器
        self.elev.grid_layout_widget[idx].setEnabled(False) # 禁用楼层按键
        self.elev.openbtn[idx].setEnabled(False) # 禁用开门键
        self.elev.closebtn[idx].setEnabled(False) # 禁用关门键

        self.openAnim(idx)
        
        if (np.array(self.elev.elev_enabled) == False).all():
            for i in range(20):
                self.elev.upbtn[i].setEnabled(False) 
                self.elev.downbtn[i].setEnabled(False)
            
            time.sleep(0.5)
            QMessageBox.information(self.elev.warnbtn[idx], "警告", "坏了，全坏了")


    def doorCtrl(self, idx, command):
        if command == OPEN:
            # 如果门是关着的且电梯是静止的
            if self.elev.door_state[idx] == CLOSE and self.elev.elev_state[idx] == STILL:
                self.elev.door_state[idx] = OPEN # 设置门的状态为开
                self.openAnim(idx)
                self.elev.anim_state[idx] = PE
        
        else:
            if self.elev.door_state[idx] == OPEN and self.elev.elev_state[idx] == STILL:
                self.elev.door_state[idx] = CLOSE # 设置门的状态为关
                self.closeAnim(idx)
                self.elev.anim_state[idx] = PE

    def openAnim(self, idx):
        self.elev.elev_anim[2 * idx].setDirection(QAbstractAnimation.Forward)
        self.elev.elev_anim[2 * idx + 1].setDirection(QAbstractAnimation.Forward)
        self.elev.elev_anim[2 * idx].start()
        self.elev.elev_anim[2 * idx + 1].start()

    def closeAnim(self, idx):
        self.elev.elev_anim[2 * idx].setDirection(QAbstractAnimation.Backward)
        self.elev.elev_anim[2 * idx + 1].setDirection(QAbstractAnimation.Backward)
        self.elev.elev_anim[2 * idx].start()
        self.elev.elev_anim[2 * idx + 1].start()
