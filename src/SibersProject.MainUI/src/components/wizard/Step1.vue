<template>
  <div class="container">
    <h2>Шаг 1</h2>
    <h3>Заполнение названия проекта, дат начала и окончания проекта, приоритета проекта</h3>
    <div class="input-group">
      <label for="name">Название проекта:</label>
      <input type="text" id="name" v-model="stepData.Name" required />
    </div>
    <div class="input-group">
      <label for="priority">Приоритет проекта:</label>
      <input
        type="number"
        id="priority"
        step="1"
        min="0"
        v-model.number="stepData.Priority"
        @keydown="filterKey"
        @input="filterInput"
        required
      />
    </div>
    <div class="input-group">
      <label for="startDate">Дата начала проекта:</label>
      <input
        type="date"
        id="startDate"
        :min="minStartDate"
        :max="maxStartDate"
        v-model="stepData.StartDate"
        required
      />
    </div>
    <div class="input-group">
      <label for="endDate">Дата окончания проекта:</label>
      <input
        type="date"
        id="endDate"
        :min="minEndDate"
        :max="maxEndDate"
        v-model="stepData.EndDate"
        required
      />
    </div>
  </div>
</template>

<script>
export default {
  props: {
    stepData: Object
  },
  computed: {
    minStartDate() {
      return null
    },
    maxStartDate() {
      return this.stepData.EndDate || null
    },
    minEndDate() {
      return this.stepData.StartDate || null
    },
    maxEndDate() {
      return null
    }
  },
  methods: {
    filterKey(e) {
      const key = e.key
      if (key === '.' || key === 'e') return e.preventDefault()
    },

    filterInput(e) {
      e.target.value = e.target.value.replace(/[^0-9]+/g, '')
    }
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
