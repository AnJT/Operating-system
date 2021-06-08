<template>
  <div class="memory">
    <el-row :gutter="20">
      <el-col :span="6">
        <div class="grid-content bg-purple">
          <span v-cloak>{{msg}}</span>
        </div>
      </el-col>
      <el-col :span="6"><div class="grid-content bg-purple"></div></el-col>
      <el-col :span="6"><div class="grid-content bg-purple"></div></el-col>
      <el-col :span="6"><div class="grid-content bg-purple"></div></el-col>
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
      name: 'hh'
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
[v-cloak] {
  display: none;
}
</style>
