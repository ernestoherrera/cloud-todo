<template>
  <div class="home">
    <TodoList :todoItems=todoItems />
  </div>
</template>

<script>
// @ is an alias to /src
import TodoList from "@/components/TodoList.vue";
import { api } from "../api/todo.js";

export default {
  name: "Home",
  components: {
    TodoList
  },
  data() {
    return {
    todoItems: []
    }
  },
  mounted(){
    console.log(`this.todoItems`, this.todoItems);

    api.getTodosByUser(2).then((data) => {
      console.log("data ", data.data);
      console.log("this.todoItems", this.todoItems);
      this.todoItems = data.data;
    });
  }
};
</script>
