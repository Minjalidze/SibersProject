<template>
  <div class="container">
    <h2>Шаг 4</h2>
    <h3>Выбор исполнителей для проекта из списка всех сотрудников</h3>
    <p>Исполнители проекта:</p>
    <div class="selected-input">
      <div v-for="(employee, index) in stepData.Employees" :key="index" class="selected-item">
        <input
          v-model="employee.name"
          @change="changeItem(employee)"
          :list="'selectedEmployeesList' + index"
          placeholder="Введите имя исполнителя"
        />
        <button @click="removeItem(index)">Удалить</button>
        <datalist :id="'selectedEmployeesList' + index">
          <option v-for="employee in employees" :key="employee.id" :value="employee.name"></option>
        </datalist>
      </div>
    </div>
    <div class="main-input">
      <input
        v-model="selectedEmployee.name"
        list="employeesList"
        @input="handleInput"
        @change="addItem"
        placeholder="Введите имя исполнителя"
      />
      <datalist id="employeesList">
        <option v-for="employee in employees" :key="employee.id" :value="employee.name"></option>
      </datalist>
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
  methods: {
    addItem() {
      const isValidName = this.employees.some(
        (employee) => employee.name === this.selectedEmployee.name
      )

      if (
        this.selectedEmployee.name &&
        isValidName &&
        !this.stepData.Employees.some((employee) => employee.name === this.selectedEmployee.name)
      ) {
        this.stepData.Employees.push(this.selectedEmployee)
        this.selectedEmployee = { id: null, name: '' }
      } else {
        this.selectedEmployee.name = ''
      }
    },
    changeItem(employee) {
      if (!this.employees.some((item) => item.name === employee.name)) {
        employee.name = ''
      }
    },
    removeItem(index) {
      this.stepData.Employees.splice(index, 1)
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
            { id: 1, name: 'lool1' },
            { id: 2, name: 'lool2' },
            { id: 3, name: 'lool3' },
            { id: 4, name: 'lool4' },
            { id: 5, name: 'lool5' }
          ]
      } catch (error) {
        console.error(error)
      }
    }
  },
  mounted() {
    this.loadEmployees()
  },
  beforeUnmount() {
    this.stepData.Employees = this.stepData.Employees.filter((employee) => employee.name !== '')
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

.selected-item {
  margin-top: 5px;
  margin-bottom: 5px;
}

.main-input input {
  width: 300px;
  padding: 5px;
  box-sizing: border-box;
}

.selected-input input {
  width: 200px;
  padding: 5px;
  box-sizing: border-box;
}

.selected-input button {
  width: 100px;
  padding: 5px;
  box-sizing: border-box;
}

p {
  padding-bottom: 5px;
}

h3 {
  margin-bottom: 33px;
}
</style>
