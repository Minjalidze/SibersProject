<template>
  <div class="buttons-container">
    <div class="nav-buttons">
      <button
        @click="changeStep(currentStep - 1)"
        :disabled="currentStep === 1"
        v-if="currentStep !== 1"
      >
        Назад
      </button>
      <button
        @click="changeStep(currentStep + 1)"
        :disabled="currentStep === totalSteps"
        v-if="currentStep !== totalSteps"
      >
        Вперед
      </button>
    </div>
    <button v-if="formCompleted" @click="submitForm">Отправить</button>
  </div>
</template>

<script>
export default {
  props: {
    currentStep: Number,
    totalSteps: Number,
    formCompleted: Boolean,
    changeStep: Function,
    submitForm: Function
  }
}
</script>

<style scoped>
.nav-buttons {
  display: flex;
  justify-content: center;
}

.buttons-container {
  display: flex;
  flex-direction: column;
  margin-bottom: 59px;
}

button {
  cursor: pointer;
  font-size: 16px;
  margin: 10px 5px;
}
</style>
