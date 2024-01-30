<template>
  <div class="wizard-container">
    <header class="header">
      <h1>Визард создания проекта</h1>
    </header>
    <main class="main">
      <StepIndicator
        :totalItems="totalSteps"
        :itemsPerPage="1"
        :currentStep="currentStep"
        @page-change="changeStep"
      />
      <component
        class="component"
        :is="currentStepComponent"
        :step-data.sync="stepData"
        @step-updated="handleStepUpdated"
      />
      <Buttons
        :currentStep="currentStep"
        :totalSteps="totalSteps"
        :formCompleted="formCompleted"
        :changeStep="changeStep"
        :submitForm="submitForm"
      />
    </main>
    <footer class="footer" />
  </div>
</template>

<script>
import axios from 'axios'
import Step1 from '@/components/wizard/Step1.vue'
import Step2 from '@/components/wizard/Step2.vue'
import Step3 from '@/components/wizard/Step3.vue'
import Step4 from '@/components/wizard/Step4.vue'
import Step5 from '@/components/wizard/Step5.vue'
import StepIndicator from '@/components/pagination/StepIndicator.vue'
import Buttons from '@/components/pagination/Buttons.vue'

export default {
  components: {
    Step1,
    Step2,
    Step3,
    Step4,
    Step5,
    StepIndicator,
    Buttons
  },
  data() {
    return {
      totalSteps: 5,
      currentStep: 1,
      stepData: {
        Name: '',
        Priority: null,
        StartDate: null,
        EndDate: null,
        ClientCompanyName: '',
        ExecutiveCompanyName: '',
        ProjectManager: null,
        Employees: [],
        documents: []
      },
      formCompleted: false
    }
  },
  computed: {
    currentStepComponent() {
      return `Step${this.currentStep}`
    }
  },
  methods: {
    changeStep(newStep) {
      this.currentStep = newStep
    },

    handleStepUpdated() {
      const isEmployeesChoosed = this.stepData.Employees.length > 0
      const isOtherDataCompleted = Object.values(this.stepData).every(
        (value) => value !== null && value !== ''
      )
      this.formCompleted = isEmployeesChoosed && isOtherDataCompleted
      console.log(this.stepData)
    },

    submitForm() {
      const startDate = new Date(this.stepData.StartDate)
      const endDate = new Date(this.stepData.EndDate)

      const sendingData = {
        Name: this.stepData.Name,
        Priority: this.stepData.Priority,
        StartDate: startDate.toISOString(),
        EndDate: endDate.toISOString(),
        ClientCompanyName: this.stepData.ClientCompanyName,
        ExecutiveCompanyName: this.stepData.ExecutiveCompanyName,
        ProjectManagerId: this.stepData.ProjectManager.id + '',
        EmployeesIds: this.stepData.Employees.map((item) => item.id.toString()),
        documents: this.stepData.documents
      }
      console.log('Form submitted with data:', sendingData)
      this.createProject(sendingData)
    },

    async createProject(data) {
      const apiUrl = 'api/Project'
      try {
        const response = await axios.post(apiUrl, data)
        console.log(response)
        alert('Проект успешно создан (подробнее в консоли)!')
      } catch (error) {
        console.error(error)
        alert('Ошибка при создании проекта (подробнее в консоли)!')
      }
    }
  },
        mounted() {
            const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoibWFpbmFkbWluaXN0cmF0b3IiLCJqdGkiOiI5Zjg4YTM2Ny02YmIwLTQ5MzYtOWNhNS03ZDRiNWI1YzU5YWEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiQWRtaW4iLCJBZG1pbiJdLCJleHAiOjE3MDY1OTA5ODUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJ9.Cfx2PWdYrNKl6XyKpjc83IQGgE7zTzRGcOON4YDN4PI';
            const header = "Authorization";

            axios.interceptors.request.use(
                (config) => {
                    config.headers[header] = `Bearer ${ token }`;
                    return config;
                },
                (error) => {
                    return Promise.reject(error);
                }
            );
        },
}
</script>

<style scoped>
.wizard-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
  width: 33vw;
  margin: 0 auto;
}

.header {
  flex: 0 0 10vh;
  display: flex;
  justify-content: center;
  align-items: center;
}

.main {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
}
</style>
