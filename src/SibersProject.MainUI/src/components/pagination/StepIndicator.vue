<template>
  <div class="step-indicators">
    <div class="progress-bar-empty"></div>
    <div class="progress-bar" :style="progressBarWidth()"></div>
    <div
      v-for="(step, index) in totalSteps"
      :key="step"
      class="step-indicator"
      @click="changeStep(step)"
      :class="{ active: step === currentStep, done: step < currentStep }"
      :style="stepStyle(index)"
    >
      {{ step }}
    </div>
  </div>
</template>

<script>
export default {
  props: {
    totalItems: {
      type: Number,
      required: true
    },
    itemsPerPage: {
      type: Number,
      required: true
    },
    currentStep: {
      type: Number,
      required: true
    }
  },
  computed: {
    totalSteps() {
      return Math.ceil(this.totalItems / this.itemsPerPage)
    }
  },
  methods: {
    changeStep(newStep) {
      this.$emit('page-change', newStep)
    },
    stepStyle(index) {
      const left = index === 0 ? 0 : `calc(100% / ${this.totalSteps - 1} * ${index})`
      return { left }
    },
    progressBarWidth() {
      const width =
        this.currentStep === 1 ? 0 : `calc(100% / ${this.totalSteps - 1} * ${this.currentStep - 1})`
      return { width }
    }
  }
}
</script>

<style scoped>
.step-indicators {
  position: relative;
  height: 30px;
  width: 100%;
  left: -15px;
  top: 77px;
}

.progress-bar {
  height: 35%;
  width: 100%;
  background-color: #00ffb3;
  position: absolute;
  left: 15px;
  transition: width 0.5s ease;
  top: 50%;
  transform: translateY(-50%);
  box-shadow: 0 0 15px rgba(73, 255, 200, 0.569);
}

.progress-bar-empty {
  height: 10%;
  width: 100%;
  background-color: #9bffe152;
  position: absolute;
  transition: width 0.5s ease;
  top: 50%;
  transform: translateY(-50%);
}

.step-indicator {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 30px;
  height: 100%;
  border: 3px solid #00ffb3;
  border-radius: 50%;
  background-color: var(--color-background);
  font-weight: bold;
  cursor: pointer;
  position: absolute;
}

.step-indicator.active {
  background-color: #00ffb3;
  color: black;
}

.step-indicator.done {
  background-color: #00ffb3;
  color: black;
}
</style>
