<template>
  <div class="memory">
    <el-row style="height: 35px">
      <el-col :span="5">
      <div>
        <a href="https://sse.tongji.edu.cn">
          <img style="height: 40px" src="https://z3.ax1x.com/2021/06/04/2Y8EWV.jpg">
        </a>
      </div>
      </el-col>
      <el-col :span="15">
<!--        <p>可恶的操作系统内存管理项目</p>-->
      </el-col>
      <el-col :span="1">
        <div>
          <a href="https://github.com/AnJT">
            <img style="height: 40px" src="https://avatars.githubusercontent.com/u/57489474?v=4">
          </a>
        </div>
      </el-col>
      <el-col :span="3">
        <div style="height: 30px">
          <div>
            <el-link :underline="false" href="https://github.com/AnJT">番茄炒鸡蛋</el-link>
          </div>

            <el-link :underline="false" href="https://github.com/AnJT">1952560安江涛</el-link>
        </div>
      </el-col>
    </el-row>
    <div class="el-divider el-divider--horizontal" style="margin-bottom: 0"><!--v-if--></div>
    <el-row :gutter="20" style="margin: 0; padding: 0">
      <el-col :span="5" style="width: 300px; height: 600px">
          <p>作业指令总数</p>
          <p class="hh_p">320</p>
          <p>每页存放指令数</p>
          <p class="hh_p">10</p>
          <p>作业占用内存页数</p>
          <p class="hh_p">4</p>
          <p>页面置换算法</p>
          <el-radio-group v-model="page_algorithm">
            <el-radio-button label="FIFO算法"></el-radio-button>
            <el-radio-button label="LRU算法"></el-radio-button>
          </el-radio-group>
          <p>下一条指令地址</p>
          <p class="hh_p">{{next_address == null? 'None': next_address}}</p>
          <p>缺页数</p>
          <p class="hh_p">{{miss_page_num}}</p>
          <p>缺页率</p>
          <p class="hh_p">{{miss_page_rate}}%</p>
      </el-col>
      <el-col :span="13">
        <p class="hh_p">内存中的页面图示</p>
        <el-row :gutter="20">
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card" :style="frame_style[0]">
                <el-header class="el-card__header">
                  <p>第{{frame[0].num == null? 'None': frame[0].num}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background: rgb(109, 178, 250);" :style="order_style[item]" v-for="item in frame[0].list" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card" :style="frame_style[1]">
                <el-header class="el-card__header">
                  <p>第{{frame[1].num == null? 'None': frame[1].num}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background: rgb(109, 178, 250);" :style="order_style[item]" v-for="item in frame[1].list" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card" :style="frame_style[2]">
                <el-header class="el-card__header">
                  <p>第{{frame[2].num == null? 'None': frame[2].num}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background: rgb(109, 178, 250);" :style="order_style[item]" v-for="item in frame[2].list" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card" :style="frame_style[3]">
                <el-header class="el-card__header">
                  <p>第{{frame[3].num == null? 'None': frame[3].num}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background: rgb(109, 178, 250);" :style="order_style[item]" v-for="item in frame[3].list" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
        </el-row>

        <el-row :gutter="20" style="padding-top: 40px">
          <el-col :span="4"></el-col>
          <el-col :span="5"><el-button @click="hh" v-bind:disabled="is_disabled" type="primary" icon="el-icon-arrow-right" round>单步执行</el-button></el-col>
          <el-col :span="5"><el-button @click="hhhh" type="primary" icon="el-icon-d-arrow-right" round>{{s_exec_name}}</el-button></el-col>
          <el-col :span="5"><el-button @click="init" v-bind:disabled="is_disabled" class="el-button el-button--warning is-round" type="primary" icon="el-icon-refresh-right" round>复位 </el-button></el-col>
          <el-col :span="9"></el-col>
        </el-row>
      </el-col>
      <el-col :span="6">
        <p class="hh_p">已执行指令</p>
          <el-table
              :data="table_data"
              highlight-current-row
              ref="table"
              height="540"
              style="width: 500px">
            <el-table-column
                prop="order"
                label=""
                width="60px">
            </el-table-column>
            <el-table-column
                prop="address"
                label="地址"
                width="70px">
            </el-table-column>
            <el-table-column
                prop="loss_page"
                label="缺页"
                width="70px">
            </el-table-column>
            <el-table-column
                prop="out_page"
                label="换出页"
                width="70px">
            </el-table-column>
            <el-table-column
                prop="in_page"
                label="换入页"
                width="70px">
            </el-table-column>
          </el-table>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  name: 'memory',
  props: {
    msg: String
  },
  data(){
    return{
      miss_page_num: 0,
      miss_page_rate: 0,
      page_algorithm: 'FIFO算法',
      frame: [{num: null, list: []}, {num: null, list: []}, {num: null, list: []}, {num: null, list: []},],
      table_data: [],
      s_exec_name: '连续执行',
      is_disabled: false,
      page_data: [],
      next_address: null,
      pre_address: null,
      fifo_queue: [0, 1, 2, 3],
      lru_queue: [0, 0, 0, 0],
      interval: '',
      frame_style: ['','','',''],
      order_style: [],
      current_row: null
    };
  },
  methods:{
    //单步执行
    hh(){
      if(this.table_data.length === 320)
        return
      this.exec()
    },
    //连续执行
    hhhh : function (){
      if(this.table_data.length === 320){
        this.s_exec_name = '连续执行'
        this.is_disabled = false
        return
      }
      this.s_exec_name = this.s_exec_name === '连续执行' ? '停止执行' : '连续执行'
      this.is_disabled = !this.is_disabled
      if(this.is_disabled === true)
        this.interval = setInterval(this.exec, 100)
      else
        clearInterval(this.interval)
    },
    //复位
    init(){
      this.miss_page_num = 0
      this.miss_page_rate = 0
      this.frame = [{num: null, list: []}, {num: null, list: []}, {num: null, list: []}, {num: null, list: []},]
      this.table_data = []
      this.s_exec_name = '连续执行'
      this.is_disabled = false
      this.next_address = Math.floor(Math.random() * 320)
      this.pre_address = null
      this.fifo_queue = [0, 1, 2, 3]
      this.lru_queue = [0, 0, 0, 0]
      this.frame_style = ['','','','']
      this.current_row = null
      for(let i = 0; i < 320; i++)
        this.order_style.push('')
    },
    //返回应该放在哪个frame里以及换出页
    FIFO(){
      let frame_num = this.fifo_queue[0]
      this.fifo_queue.shift()
      this.fifo_queue.push(frame_num)
      //同时更新lru
      this.lru_queue[frame_num] = new Date().getTime()
      return [frame_num, this.frame[frame_num].num]
    },
    LRU(){
      let frame_num = 0
      for(let i = 1; i < this.lru_queue.length; i++){
        if(this.lru_queue[i] < this.lru_queue[frame_num])
          frame_num = i
      }
      this.lru_queue[frame_num] = new Date().getTime()
      //同时更新fifo
      this.fifo_queue.splice(this.fifo_queue.indexOf(frame_num), 1)
      this.fifo_queue.push(frame_num)
      return [frame_num, this.frame[frame_num].num]
    },
    //执行一次
    exec() {
      if (this.table_data.length === 320) {
        this.s_exec_name = '连续执行'
        this.is_disabled = false
        clearInterval(this.interval)
      }
      else {
        //恢复上一条指令的样式
        this.frame_style = ['','','','']
        this.order_style[this.pre_address] = ''

        let page_num = Math.floor(this.next_address / 10)
        //用于判断指令是否在内存中
        let is_find = false

        for (let i = 0; i < this.frame.length; i++) {
          if (page_num === this.frame[i].num) {
            this.lru_queue[i] = new Date().getTime()
            is_find = true
            break
          }
        }
        this.order_style[this.next_address] = {background: 'Orchid'}
        if (is_find === false) {
          this.miss_page_num++
          this.miss_page_rate = Math.floor(this.miss_page_num * 100 / (this.table_data.length + 1))
          let result
          if (this.page_algorithm === 'FIFO算法')
            result = this.FIFO()
          else
            result = this.LRU()
          this.frame_style[result[0]] = {background: 'cornflowerblue'}
          this.frame[result[0]].num = page_num
          this.frame[result[0]].list = this.page_data[page_num]
          let out_page = result[1] == null ? '' : result[1]
          this.table_data.unshift({
            order: this.table_data.length,
            address: this.next_address,
            loss_page: 'Yes',
            out_page: out_page,
            in_page: page_num
          })
        } else {
          this.table_data.unshift({
            order: this.table_data.length, address: this.next_address, loss_page: 'No', out_page: '', in_page: ''
          })
        }
        this.setCurrent()
        this.getNextAddress()
      }
    },
    //产生下一条指令地址
    getNextAddress() {
      this.pre_address = this.next_address
      let rand = Math.random()
      //顺序执行
      if (rand < 0.5) {
        this.next_address++
        this.next_address %= 320
      }
      //25%的概率向后跳
      else if (rand < 0.75) {
        let dx = Math.floor(Math.random() * 160)
        this.next_address = (this.next_address + dx) % 320
      }
      //25%的概率向前跳
      else {
        let dx = Math.floor(Math.random() * 160)
        this.next_address = (this.next_address - dx + 320) % 320
      }
    },
    //设置table当前行
    setCurrent() {
      this.$refs.table.setCurrentRow(this.table_data[0]);
    }
  },
  created() {
    //初始化页表
    for(let i = 0; i < 32; i++){
      let arr = []
      for(let j = i * 10; j < (i + 1) * 10; j++ )
        arr.push(j)
      this.page_data.push(arr)
    }
    //产生第一条指令
    this.next_address = Math.floor(Math.random() * 320)
    this.lru_queue = [0, 0, 0, 0]
    for(let i = 0; i < 320; i++)
      this.order_style.push('')
  }
}
</script>

<style>
.transition-box {
  width: 100px;
  height: 20px;
  text-align: center;
  color: #fff;
  border: 1px solid;
  margin: 10px auto;
  border-radius: 4px;
}

.hh_p {
  display: block;
  font-size: 1.17em;
  margin-block-start: 1em;
  margin-block-end: 1em;
  margin-inline-start: 0;
  margin-inline-end: 0;
  font-weight: bold;
}
</style>
