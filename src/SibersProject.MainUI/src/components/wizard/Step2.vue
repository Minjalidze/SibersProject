<template>
  <div class="container">
    <h2>Шаг 2</h2>
    <h3>Заполнение компании-заказчика и компании-исполнителя</h3>
    <div class="input-group">
      <label for="customer">Название компании-заказчика:</label>
      <input type="text" id="customer" v-model="stepData.ClientCompanyName" required />
    </div>
    <div class="input-group">
      <label for="performer">Название компании-исполнителя:</label>
      <input type="text" id="performer" v-model="stepData.ExecutiveCompanyName" required />
    </div>
  </div>
</template>

<script>
export default {
  props: {
    stepData: Object
  },
  watch: {
    stepData: {
      handler(newValue) {
        this.$emit('step-updated', newValue)
      },
      deep: true
    }
  }
}
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.input-group {
  margin-bottom: 20px;
}

h3 {
  margin-bottom: 33px;
}

label {
  text-align: center;
  display: block;
  margin-bottom: 5px;
}

input {
  width: 300px;
  padding: 5px;
  box-sizing: border-box;
}
</style>
