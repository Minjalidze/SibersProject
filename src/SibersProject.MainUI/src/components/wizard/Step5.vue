<template>
  <div class="upload-container">
    <h2>Шаг 5</h2>
    <h3>Загрузка документов проекта</h3>
    <div
      class="drop-zone"
      @dragover.prevent="dragOver"
      @drop.prevent="dropFiles"
      @click="openFileInput"
    >
      <p v-if="this.stepData.documents.length === 0">
        Перетащите файлы сюда или нажмите для выбора файлов
      </p>
      <div v-else>
        <p v-for="(file, index) in this.stepData.documents" :key="index">
          Загружен файл: {{ file.name }} ({{ formatBytes(file.size) }})
          <button @click="removeFile(index, $event)">Удалить</button>
        </p>
      </div>
      <input type="file" ref="fileInput" @change="selectFiles" multiple style="display: none" />
    </div>
  </div>
</template>

<script>
export default {
  props: {
    stepData: Object
  },
  methods: {
    dragOver(event) {
      event.dataTransfer.dropEffect = 'copy'
    },
    dropFiles(event) {
      event.preventDefault()
      const files = event.dataTransfer.files
      if (files.length > 0) {
        this.handleFiles(files)
      }
    },
    selectFiles() {
      const fileInput = this.$refs.fileInput
      const files = fileInput.files
      if (files.length > 0) {
        this.handleFiles(files)
      }
    },
    handleFiles(files) {
      for (let i = 0; i < files.length; i++) {
        this.stepData.documents.push(files[i])
      }
    },
    removeFile(index, event) {
      event.stopPropagation()
      this.stepData.documents.splice(index, 1)
    },
    formatBytes(bytes, decimals = 2) {
      if (bytes === 0) return '0 Bytes'
      const k = 1024
      const dm = decimals < 0 ? 0 : decimals
      const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']

      const i = Math.floor(Math.log(bytes) / Math.log(k))

      return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i]
    },
    openFileInput() {
      this.$refs.fileInput.click()
    }
  },
  beforeDestroy() {
    this.stepData.documents = this.stepData.documents.filter((file) => file.name !== '')
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
.upload-container {
  text-align: center;
}

.drop-zone {
  border: 2px dashed #595959;
  padding: 33% 15%;
  text-align: center;
  cursor: pointer;
}

.drop-zone p {
  margin: 0;
}

.drop-zone:hover {
  border-color: #00ffb3;
}

button {
  margin-left: 10px;
}

h3 {
  margin-bottom: 33px;
}
</style>
