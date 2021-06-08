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
          <a href="https://github.com/AnJT">
            <p style="margin: 0">番茄炒鸡蛋</p>
            <p style="margin: 0">1952560安江涛</p>
          </a>
        </div>
      </el-col>
    </el-row>
    <div class="el-divider el-divider--horizontal" style="margin-bottom: 0"><!--v-if--></div>
    <el-row :gutter="20" style="margin: 0; padding: 0">
      <el-col :span="5" style="width: 300px; background-color: rgb(238, 241, 246); box-shadow: rgba(0, 0, 0, 0.1) 0 2px 12px 0; height: 600px">
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
          <p>指令执行顺序</p>
          <div class="el-divider el-divider--horizontal"><!--v-if--></div>
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
              <el-container class="el-card is-always-shadow box-card">
                <el-header class="el-card__header">
                  <p>第{{page_0}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                    <div class="transition-box" style="background-color: rgb(109, 178, 250);" v-for="item in list_0" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card">
                <el-header class="el-card__header">
                  <p>第{{page_1}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background-color: rgb(109, 178, 250);" v-for="item in list_1" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card">
                <el-header class="el-card__header">
                  <p>第{{page_2}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background-color: rgb(109, 178, 250);" v-for="item in list_2" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
          <el-col :span="6"><div class="grid-content bg-purple">
            <div class="common-layout">
              <el-container class="el-card is-always-shadow box-card">
                <el-header class="el-card__header">
                  <p>第{{page_3}}页</p>
                </el-header>
                <div class="el-divider el-divider--horizontal" style="margin: 0"><!--v-if--></div>
                <div class="el-card__body" style="height: 340px; padding: 10px">
                  <div class="transition-box" style="background-color: rgb(109, 178, 250);" v-for="item in list_3" :key="item">{{item}}</div>
                </div>
              </el-container>
            </div>
          </div></el-col>
        </el-row>

        <el-row :gutter="20" style="padding-top: 40px">
          <el-col :span="5"></el-col>
          <el-col :span="5"><el-button type="primary" icon="el-icon-edit" round>单步执行</el-button></el-col>
          <el-col :span="5"><el-button type="primary" icon="el-icon-edit" round>连续执行</el-button></el-col>
          <el-col :span="5"><el-button type="primary" icon="el-icon-edit" round>复位 </el-button></el-col>
          <el-col :span="8"></el-col>
        </el-row>
      </el-col>
      <el-col :span="6">
          <p class="hh_p">已执行指令</p>
          <el-table
              :data="tableData"
              height="540"
              style="width: 500px">
            <el-table-column
                prop="order"
                label=""
                width="40px">
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
      name: 'hh',
      miss_page_num: 0,
      miss_page_rate: 0,
      page_algorithm:'FIFO算法',
      page_0:'None',
      page_1:'None',
      page_2:'None',
      page_3:'None',
      list_0:[],
      list_1:[],
      list_2:[],
      list_3:["1","2",3,4,5,6,7,8,9,10],
      tableData: []
    };
  },
  methods:{
    hh(){
      alert('实在是拦不住！')
    },
    hhh : function (){

    }
  },
  mounted() {
    setInterval(() => {
      this.counter++
    }, 1000)
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
  margin-inline-start: 0px;
  margin-inline-end: 0px;
  font-weight: bold;
}

</style>
