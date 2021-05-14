import sys

from PyQt5.QtWidgets import QApplication

from elevator import Ui

if __name__ == '__main__':
    app = QApplication(sys.argv)
    ui = Ui()
    ui.show()
    sys.exit(app.exec_())
