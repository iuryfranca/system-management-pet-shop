window.flowbiteInterop = {
  initializeFlowbite: function () {
    return initFlowbite();
  },

  getModal: function (modalId) {
    const $targetEl = document.getElementById(modalId);

    const options = {
      placement: "center",
      backdrop: "static",
      backdropClasses: "bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40",
      closable: true,
    };

    const instanceOptions = {
      id: modalId,
      override: true,
    };

    const modal = new Modal($targetEl, options, instanceOptions);
    console.log(modal);
    return modal;
  },

  showModal: function (modalId) {
    const modal = this.getModal(modalId);
    modal.show();
  },

  closeModal: function (modalId) {
    const modal = this.getModal(modalId);
    modal.hide();
  },
};

window.downloadFile = function (filename, content, contentType) {
  const blob = new Blob([content], { type: contentType });
  const link = document.createElement('a');
  link.href = window.URL.createObjectURL(blob);
  link.download = filename;
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  window.URL.revokeObjectURL(link.href);
};

window.downloadPDFReport = function (filename, reportData, reportType) {
  const { jsPDF } = window.jspdf;
  const doc = new jsPDF();
  
  // Configurações do documento
  doc.setFont("helvetica");
  
  // Título do relatório
  let title = "";
  switch (reportType) {
    case 1:
      title = "Relatório de Serviços por Tipo de Animal";
      break;
    case 2:
      title = "Relatório de Ocorrências de Visitas de Animais";
      break;
    case 3:
      title = "Relatório de Serviços por Funcionário";
      break;
  }
  
  // Adicionar título
  doc.setFontSize(18);
  doc.setTextColor(40, 40, 40);
  doc.text(title, 20, 20);
  
  // Data de geração
  doc.setFontSize(10);
  doc.setTextColor(100, 100, 100);
  const now = new Date();
  doc.text(`Gerado em: ${now.toLocaleString('pt-BR')}`, 20, 30);
  
  // Configurar colunas e dados da tabela
  let columns = [];
  let rows = [];
  
  switch (reportType) {
    case 1:
      columns = ['Tipo Animal', 'Qtd Serviços', 'Valor Total', 'Valor Médio'];
      rows = reportData.map(item => [
        item.TipoAnimal,
        item.QuantidadeServicos,
        `R$ ${(item.ValorTotal || 0).toFixed(2)}`,
        `R$ ${(item.ValorMedio || 0).toFixed(2)}`
      ]);
      break;
    case 2:
      columns = ['Nome Animal', 'Tipo', 'Dono', 'Qtd Visitas', 'Primeira Visita', 'Última Visita'];
      rows = reportData.map(item => [
        item.NomeAnimal,
        item.TipoAnimal,
        item.DonAnimal,
        item.QuantidadeVisitas,
        item.PrimeiraVisita ? new Date(item.PrimeiraVisita).toLocaleDateString('pt-BR') : '-',
        item.UltimaVisita ? new Date(item.UltimaVisita).toLocaleDateString('pt-BR') : '-'
      ]);
      break;
    case 3:
      columns = ['Funcionário', 'Cargo', 'Qtd Serviços', 'Valor Total', 'Valor Médio', 'Primeiro Serviço', 'Último Serviço'];
      rows = reportData.map(item => [
        item.NomeFuncionario,
        item.CargoFuncionario,
        item.QuantidadeServicos,
        `R$ ${(item.ValorTotalServicos || 0).toFixed(2)}`,
        `R$ ${(item.ValorMedioServicos || 0).toFixed(2)}`,
        item.PrimeiroServico ? new Date(item.PrimeiroServico).toLocaleDateString('pt-BR') : '-',
        item.UltimoServico ? new Date(item.UltimoServico).toLocaleDateString('pt-BR') : '-'
      ]);
      break;
  }
  
  // Gerar tabela
  doc.autoTable({
    startY: 40,
    head: [columns],
    body: rows,
    theme: 'grid',
    styles: {
      font: 'helvetica',
      fontSize: 8,
      cellPadding: 3,
      textColor: [40, 40, 40],
    },
    headStyles: {
      fillColor: [41, 128, 185],
      textColor: [255, 255, 255],
      fontStyle: 'bold',
      fontSize: 9
    },
    alternateRowStyles: {
      fillColor: [245, 245, 245]
    },
    margin: { top: 40, left: 10, right: 10 },
    tableWidth: 'auto',
    columnStyles: {
      0: { cellWidth: 'auto' },
      1: { cellWidth: 'auto' },
      2: { cellWidth: 'auto' }
    }
  });
  
  // Adicionar total de registros
  const finalY = doc.lastAutoTable.finalY + 10;
  doc.setFontSize(10);
  doc.setTextColor(100, 100, 100);
  doc.text(`Total de registros: ${rows.length}`, 20, finalY);
  
  // Rodapé
  const pageCount = doc.internal.getNumberOfPages();
  for (let i = 1; i <= pageCount; i++) {
    doc.setPage(i);
    doc.setFontSize(8);
    doc.setTextColor(150, 150, 150);
    doc.text(
      `Sistema Pet Shop - Página ${i} de ${pageCount}`,
      doc.internal.pageSize.width / 2,
      doc.internal.pageSize.height - 10,
      { align: 'center' }
    );
  }
  
  // Download do PDF
  doc.save(filename);
};
