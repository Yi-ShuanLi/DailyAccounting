using CSV;
using DailyAccounting.Models;
using DailyAccounting.Models.DTOs;
using DailyAccounting.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DailyAccounting.Contract.AddARecordContract;

namespace DailyAccounting.Presenters
{
    internal class RecordPresenter : IAddRecordPresenter
    {
        String recordPath = ConfigurationManager.AppSettings["CSVPath"].ToString();
        IAddRecordView addRecordView;
        IDataModelRepository dataModelRepository;
        IRecordRepository recordOneRepository;
        public RecordPresenter(IAddRecordView view)
        {
            this.addRecordView = view;
            this.dataModelRepository = new DataModelRepository();
            this.recordOneRepository = new RecordOneRepository();
        }

        public void ChangeCategoryRequest(string categoryName)
        {
            List<string> result = dataModelRepository.GetPurposeDataSource(categoryName);
            addRecordView.ChangeCategoryResponse(result);
        }

        public void GetComboBoxDataRequest()
        {
            DataModelDTO dataModelDTO = new DataModelDTO();
            dataModelDTO.Category = dataModelRepository.GetCategoryDataSource();
            dataModelDTO.Purpose = dataModelRepository.GetPurposeDataSource(dataModelDTO.Category[0]);
            dataModelDTO.PayWay = dataModelRepository.GetPayWayDataSource();
            dataModelDTO.Member = dataModelRepository.GetMemberDataSource();

            addRecordView.GetComboBoxDataResponse(dataModelDTO);
        }

        public void SaveRecord(RecordModelDTO recordModelDTO)
        {
            string day = recordModelDTO.Day;
            string amount = recordModelDTO.Amount;
            string category = recordModelDTO.Category;
            string purpose = recordModelDTO.Purpose;
            string payWay = recordModelDTO.PayWay;
            string member = recordModelDTO.Member;
            string image1Guid = Guid.NewGuid().ToString();
            string image2Guid = Guid.NewGuid().ToString();
            string imageURL1 = Path.Combine(recordPath, $"{day}\\Pictures\\40x40_{image1Guid}.jpg");
            string imageURL2 = Path.Combine(recordPath, $"{day}\\Pictures\\40x40_{image2Guid}.jpg");
            string imageFilePath = Path.Combine(recordPath, $"{day}\\Pictures");

            if (!Directory.Exists(imageFilePath))
            {
                Directory.CreateDirectory(imageFilePath);
            }
            recordModelDTO.BitmapImage1.Save(imageURL1);
            recordModelDTO.BitmapImage2.Save(imageURL2);

            Bitmap bitmap1 = ImageCompress.Compress((Bitmap)recordModelDTO.BitmapImage1);
            Bitmap bitmap1_40x40 = ImageCompress.Compress((Bitmap)recordModelDTO.BitmapImage1, 40, 40);
            Bitmap bitmap2 = ImageCompress.Compress((Bitmap)recordModelDTO.BitmapImage2);
            Bitmap bitmap2_40x40 = ImageCompress.Compress((Bitmap)recordModelDTO.BitmapImage2, 40, 40);
            bitmap1_40x40.Save(imageURL1);
            bitmap1.Save(Path.Combine(recordPath, $"{day}\\Pictures\\50L_{image1Guid}.jpg"));
            bitmap2_40x40.Save(imageURL2);
            bitmap2.Save(Path.Combine(recordPath, $"{day}\\Pictures\\50L_{image2Guid}.jpg"));

            RecordModel recordModel = new RecordModel(day, amount, category, purpose, payWay, member, imageURL1, imageURL2);
            recordOneRepository.CreateRecord(recordModel);
            bitmap1_40x40.Dispose();
            bitmap1.Dispose();
            bitmap2_40x40.Dispose();
            bitmap2.Dispose();
            recordModelDTO.BitmapImage1.Dispose();
            recordModelDTO.BitmapImage2.Dispose();
            GC.Collect();
        }
    }
}
