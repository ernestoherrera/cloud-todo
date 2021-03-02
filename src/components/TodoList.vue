
<template>
  <div class="container">
    <table class="table table-striped table-hover">
      <thead>
        <tr>
          <th scope="col">Number</th>
          <th scope="col">Done</th>
          <th scope="col">Action</th>
        </tr>
      </thead>
      <tbody>
        <template v-for="(todoitem, index) in todoItems">
          <TodoItem
            :index=index
            :key="todoitem.id"
            :userid="todoitem.userid"
            :description="todoitem.description"
            :done="todoitem.done"
          />
        </template>
      </tbody>
    </table>
  </div>
</template>

<script>
import TodoItem from "./TodoItem";

export default {
  name: "TodoList",
  components: {
    TodoItem,
  },
  props: {
    todoItems: Array,
  },
};
</script>