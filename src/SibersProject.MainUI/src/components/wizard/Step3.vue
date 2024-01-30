<template>
  <div class="container">
    <h2>Шаг 3</h2>
    <h3>Выбор руководителя проекта из списка всех сотрудников</h3>
    <div class="input-group">
      <p>Руководитель проекта:</p>

      <div class="main-input">
        <input
          v-model="selectedEmployee.name"
          list="employeesList"
          @input="handleInput"
          @change="addItem"
          placeholder="Введите имя руководителя"
        />
        <datalist id="employeesList">
          <option v-for="employee in employees" :key="employee.id" :value="employee.name"></option>
        </datalist>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  props: {
    stepData: Object
  },
  data() {
    return {
      employees: [],
      selectedEmployee: { id: null, name: '' }
    }
  },
  mounted() {
    this.loadEmployees()
    if (this.stepData.ProjectManager) {
      this.selectedEmployee = { ...this.stepData.ProjectManager }
    }
  },
  methods: {
    addItem() {
      const isValidName = this.employees.some(
        (employee) => employee.name === this.selectedEmployee.name
      )

      if (this.selectedEmployee.name && isValidName) {
        this.stepData.ProjectManager = this.selectedEmployee
      } else {
        this.selectedEmployee.name = ''
      }
    },
    handleInput() {
      const selectedEmployee = this.employees.find(
        (item) => item.name === this.selectedEmployee.name
      )
      if (selectedEmployee) {
        this.selectedEmployee = { ...selectedEmployee }
      }
    },

    async loadEmployees() {
        const apiUrl = '/api/Employee'
      try {
        const response = await axios.get(apiUrl)

        if (response.data instanceof Object) this.employees = response.data
        else
          this.employees = [
            { id: 1, name: 'lol1' },
            { id: 2, name: 'lol2' },
            { id: 3, name: 'lol3' },
            { id: 4, name: 'lol4' },
            { id: 5, name: 'lol5' }
          ]
      } catch (error) {
        console.error(error)
      }
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

.main-input input {
  width: 300px;
  padding: 5px;
  box-sizing: border-box;
}

p {
  padding-bottom: 5px;
  text-align: center;
}

h3 {
  margin-bottom: 33px;
}
</style>
