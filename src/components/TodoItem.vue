<template>
  <tr>
    <td width="8%">
      {{index}}
    </td>
    <td width="8%">
      <span class="form-check">
        <input class="form-check-input" type="checkbox" :value="done" id="todo-done" /> 
        </span>
    </td>
    <td align="left">{{description}}</td>   
  </tr>
</template>

<script>
export default {
    name: "TodoItem",
    props:{
        index: Number,
        done: Boolean,
        description: String,
        userid: Number,
        todoid: Number
    }
}
</script>