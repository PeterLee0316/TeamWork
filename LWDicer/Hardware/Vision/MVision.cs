﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using LWDicer.Layers;
using static LWDicer.Layers.DEF_Vision;
using static LWDicer.Layers.DEF_Error;
using static LWDicer.Layers.DEF_Common;
using Matrox.MatroxImagingLibrary;

namespace LWDicer.Layers
{
    public class MVision : MObject,IVision,IDisposable
    {
        public bool m_bSystemInit { get; set; }
        private bool m_bErrorPrint;
        private bool m_bSaveErrorImage;

        public int m_iCurrentViewNum { get; set; }
        public int m_iHairLineWidth { get; set; }
        public int m_iMarkROIWidth { get; set; }
        public int m_iMarkROIHeight { get; set; }

        private CVisionRefComp m_RefComp;
        private CCameraData m_Data;

        public MVision(CObjectInfo objInfo, CVisionRefComp refComp,CCameraData data)
            : base(objInfo)
        {
            m_RefComp           = refComp;            
            SetData(data);

            m_bSystemInit       = false;
            m_bErrorPrint       = false;
            m_bSaveErrorImage   = false;

            m_iHairLineWidth    = DEF_HAIRLINE_NOR;
            m_iMarkROIWidth     = DEF_MARK_WIDTH_NOR;
            m_iMarkROIHeight    = DEF_MARK_HEIGHT_NOR;            

            Initialize(DEF_MAX_CAMERA_NO);
        }

        public void Dispose()
        {
            
        }

        ~MVision()
        {
            Dispose();
        }
        public int SetData(CCameraData source)
        {
            m_Data = ObjectExtensions.Copy(source);
            return SUCCESS;
        }
        /// <summary>
        /// Initialize: Mechanical Layer Vision을 초기화 한다.
        /// </summary>
        /// <param name="iCameraNum"></param>
        /// <returns></returns>
        public int Initialize(int iCameraNum)
        {

#if SIMULATION_VISION
                return SUCCESS;
#endif
            m_bSystemInit = true;
            return SUCCESS;
        }
        /// <summary>
        /// CloseVisionSystem : 비전 시스템을 해제한다.
        /// </summary>
        public void CloseVisionSystem()
        {
#if SIMULATION_VISION
                return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            // Camera Distroy
            for (int iIndex = 0; iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                HaltVideo(iIndex);
                
                m_RefComp.View[iIndex].FreeDisplay();
                m_RefComp.Camera[iIndex].FreeCamera();
            }
            
            // System Distroy
            m_RefComp.System.freeSystems();

            m_bSystemInit = false;
        }
   
        /// <summary>
        /// 새 Model 에 대한 Model Data 를 Load 한다.
        /// </summary>
        /// <param name="strModelFilePath"></param>
        /// <returns></returns>
        public int ChangeModel(string strModelFilePath)
        {
            return LoadParameter(strModelFilePath);
        }

        //  파일에서 데이타를 로드 한다.	     
        //  DB를 사용하기 때문에 필요 없음 삭제 예정   
        public int LoadParameter(string strModelFilePath)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            int iResult;
            int i = 0;

            if (m_RefComp.System == null)
                return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (m_RefComp.Camera == null)
                return GenerateErrorCode(ERR_VISION_CAMERA_FAIL);

            //for (i = 0; i < DEF_MAX_CAMERA_NO; i++)
            //{
            //    if (isValidCameraNo(i))
            //    {
            //        iResult = m_RefComp.Camera[i].LoadCameraData(strModelFilePath);
            //        if (iResult > 0) return iResult;
            //    }
            //    else
            //    {
            //        continue;
            //    }

            //    // Model Mark Read
            //    m_RefComp.Camera[i].LoadSearchData(strModelFilePath);
            //}

            return SUCCESS;
        }
        /// <summary>
        /// isValidCameraNo : 카메라의 번호가 유효한지 확인한다.
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <returns></returns>
        bool isValidCameraNo(int iCamNo)
        {
#if SIMULATION_VISION
                return true;
#endif

            int i = 0;
            for (i = 0; i < DEF_MAX_CAMERA_NO; i++)
            {
                if (m_RefComp.Camera[i].GetCamID() == iCamNo)
                    return true;
            }
            
            return true;
        }
        bool isValidPatternMarkNo(int iModelNo)
        {
            if (iModelNo < 0 || iModelNo >= DEF_USE_SEARCH_MARK_NO)
                return false;

            return true;
        }

        /// <summary>
        /// DestroyLocalView : Display하고 있는 객체와 카메라의 영상 연결을 해제한다.
        /// </summary>
        /// <param name="iCamNo": 카메라 번호></param> 
        public int DestroyLocalView(int iCamNo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (m_RefComp.View[iCamNo].IsLocalView())
            {
                m_RefComp.View[iCamNo].DestroyLocalView();
            }

            return SUCCESS;
        }

        /// <summary>
        /// InitialLocalView : 카메라 영상을 객체 Handle로 연결함
        /// </summary>
        /// <param name="iCamNo": 카메라 번호></param>
        /// <param name="pObject": Display할 객체의 Handle값></param>        	     
        public int InitialLocalView(int iCamNo, IntPtr pObject)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);
            // 설정할 Camera 번호가 Max 를 초과 여부 확인
            if (iCamNo > DEF_MAX_CAMERA_NO) return GenerateErrorCode(ERR_VISION_CAMERA_FAIL);

            // 설정할 객체의 Handle값이 다른 View에 있으면 빠져 나감.
            for (int iIndex=0;iIndex < DEF_MAX_CAMERA_NO; iIndex++)
            {
                // 바꿀 View와 현재 View를 비교함.
                if (m_RefComp.View[iIndex].GetViewHandle()== pObject)
                {
                    return SUCCESS;
                }
            }
            // View를 Display로 등록한다.
            // View에 맞쳐 Zoom 설정을 한다
            // Mil의 SelectDisplayWindow 함수로 등록한다. 
            m_RefComp.View[iCamNo]. SetDisplayWindow(pObject);
            m_iCurrentViewNum = iCamNo;

            return SUCCESS;
        }
        public int ChangeLocalView(int iCamNo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);
            // 설정할 Camera 번호가 Max 를 초과 여부 확인
            if (iCamNo > DEF_MAX_CAMERA_NO) return GenerateErrorCode(ERR_VISION_CAMERA_FAIL);

            IntPtr pHandle = m_RefComp.View[iCamNo].GetViewHandle();
            m_RefComp.View[iCamNo].SetDisplayWindow(pHandle);
            m_iCurrentViewNum = iCamNo;

            return SUCCESS;
        }
        /// <summary>
        /// DisplayImage : MIL Image를 객체에 Dispaly함
        /// </summary>
        /// <param name="image": MIL Image></param>
        /// <param name="handle": Display할 객체의 Handle값></param>
        public void DisplayViewImage(MIL_ID image,IntPtr handle)
        {
#if SIMULATION_VISION
                return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            int iCamNo = 0;
            m_RefComp.View[iCamNo].DisplayImage(image, handle);
        }

        /// <summary>
        /// Grab : Grab Operation 을 수행한다.
        /// </summary>
        /// <param name="iCamNo": Grab할 카메라 선택></param>
        public void Grab(int iCamNo)
        {
#if SIMULATION_VISION
                return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            m_RefComp.Camera[iCamNo].SetTrigger();
        }

        /// <summary>
        /// GetCameraPixelSize : Camera Pixel 가로 세로 개수를 읽음
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <returns></returns>
        public Size GetCameraPixelSize(int iCamNo)
        {
#if SIMULATION_VISION
                return new Size();
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return new Size();

            return m_RefComp.Camera[iCamNo].GetCameraPixelSize();
        }


        /// <summary>
        /// GetGrabImage : Grab된 Image를 가져온다.
        /// </summary>
        /// <param name="iCamNo": Grab 카메라 선택></param>
        public MIL_ID GetGrabImage(int iViewNo)
        {
#if SIMULATION_VISION
                return MIL.M_NULL;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return MIL.M_NULL;

            return m_RefComp.View[iViewNo].GetImage();
        }

        //        public void DisplayMarkImage(int iViewNo, IntPtr pDisplayHandle )
        //        {
        //#if SIMULATION_VISION
        //                return;
        //#endif
        //            // Vision System이 초기화 된지를 확인함
        //            if (m_bSystemInit == false) return;

        //            m_RefComp.View[iViewNo].GetMarkModelImage();
        //        }
        

        /// <summary>
        /// ConnectCam : Camera 와 View Window 를 연결한다.
        /// </summary>
        /// <param name="iCamNo"></param>
        public void ConnectCam(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            // Callback에 Display View함수를 등록한다
            m_RefComp.Camera[iCamNo].SelectView(m_RefComp.View[iCamNo]);
            // System 에 Display를 연결한다.
            m_RefComp.System.SelectCamera(m_RefComp.Camera[iCamNo]);
            m_RefComp.System.SelectView(m_RefComp.View[iCamNo]);

        }


        /// <summary>
        /// SelectCamera : Camera와 View와 연결함
        /// </summary>
        /// <param name="iCamNo": 카메라 번호></param>
        /// <param name="iViewNo": View 번호></param>
        /// <returns></returns>
        public int SelectCamera(int iCamNo, int iViewNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            // View 객체의 크기와 카메라 영상의 크기를 비율로 맞춘다
            m_RefComp.View[iViewNo].SelectCamera(m_RefComp.Camera[iCamNo]);
            return SUCCESS;
        }

        //Check Enabled Model & Recognition Area	     
        public int CheckModel(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (isValidPatternMarkNo(iModelNo))
            {
                if (m_RefComp.Camera[iCamNo].GetSearchData(iModelNo).m_bIsModel)
                    return SUCCESS;
                else
                    return GenerateErrorCode(ERR_VISION_PATTERN_NONE);
            }

            return SUCCESS;
        }

        /// <summary>
        /// DeleteMark : 등록한 Pattern Mark를 삭제한다.
        /// </summary>
        /// <param name="iCamNo": 카메라 번호></param>
        /// <param name="iModelNo": Pattern Model 번호></param>
        public void DeleteMark(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
                return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (isValidPatternMarkNo(iModelNo))
            {
                m_RefComp.Camera[iCamNo].DeleteSearchModel(iModelNo);
            }
        }
        
        /// <summary>
        /// Camera의 이미지를 적용 모델 번호와 Maching Score를 이름으로 저장한다.
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Number></param>
        /// <param name="dScore": Pattern Score></param>
        /// <returns></returns>
        public int SaveImage(int iCamNo, int iModelNo=0, double dScore=0)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            DateTime time = DateTime.Now;
            string strDirName = DEF_IMAGE_LOG_FILE + String.Format("{0:yyyy_MM_dd}",time); 

            if (!Directory.Exists(strDirName))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(strDirName);
                if (!DirInfo.Exists)
                {
                    return GenerateErrorCode(ERR_VISION_FOLDER_FAIL);
                }
            }
            string strFileName = String.Format("LogImage {0:HH.mm.ss.fff} [Cam{1:0}_Mark{2:0} Score_{3:0.00}].bmp",
                                               time, iCamNo + 1, iModelNo, dScore);

            m_RefComp.View[iCamNo].SaveImage(strDirName+"\\"+strFileName);

            return SUCCESS;
        }

        public int SaveImage(int iModelNo=0, double dScore=0)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            DateTime time = DateTime.Now;
            string strDirName = DEF_IMAGE_LOG_FILE + String.Format("{0:yyyy_MM_dd}", time);

            if (!Directory.Exists(strDirName))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(strDirName);
                if (!DirInfo.Exists)
                {
                    return GenerateErrorCode(ERR_VISION_FOLDER_FAIL);
                }
            }

            string strFileName = String.Format("LogImage {0:HH.mm.ss.fff} [Cam{1:0}_Mark{2:0} Score_{3:0.00}].bmp",
                                               time, m_iCurrentViewNum + 1, iModelNo, dScore);

            m_RefComp.View[m_iCurrentViewNum].SaveImage(strDirName + "\\" + strFileName);

            return SUCCESS;

        }

        public int SaveImage(int iCamNo, string strFileName)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);



            m_RefComp.View[iCamNo].SaveImage(strFileName + $"_CAM_{iCamNo}.bmp");

            return SUCCESS;
        }

        /// <summary>
        /// SaveModelImage: Model Image를 저장함.
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public int SaveModelImage(int iCamNo, string strPath, string strName)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!Directory.Exists(strPath))
            {
                DirectoryInfo DirInfo = Directory.CreateDirectory(strPath);
                if (!DirInfo.Exists)
                {
                    return GenerateErrorCode(ERR_VISION_FOLDER_FAIL);
                }
            }

            m_RefComp.View[iCamNo].SaveImage(strPath + strName);

            return SUCCESS;

        }

        // Delete Old Error Image Files
        public int DeleteOldImageFiles()
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            return SUCCESS;
        }

        // Enable or Disable "Save Error Image" Fuction.
        public void EnableSaveErrorImage(bool bFlag = false)
        {
#if SIMULATION_VISION
                return;
#endif
            m_bSaveErrorImage = bFlag;
        }

        // Get Grab Settling Time.
	    
        public int GetGrabSettlingTime(int iCamNo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            return SUCCESS;
        }

        // Set Grab Settling Time.
	 
        public void SetGrabSettlingTime(int iCamNo, int iGrabSettlingTime)
        {
#if SIMULATION_VISION
                return;
#endif
            return;
        }

        // Get Camera Change Time.	     
        public int GetCameraChangeTime(int iCamNo)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            return SUCCESS;
        }

        // Set Camera Change Time.
        public void SetCameraChangeTime(int iCamNo, int iCameraChangeTime)
        {
            return;
        }

        /// <summary>
        /// ReLoadPatternMark : Pattern을 SearchData로 부터 재 등록한다.
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iTypeNo"></param>
        /// <param name="pSData"></param>
        /// <returns></returns>
        public int ReLoadPatternMark(int iCamNo,
                                     int iTypeNo,
                                     CSearchData pSData)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            if (m_RefComp.Camera == null) return GenerateErrorCode(ERR_VISION_CAMERA_FAIL);
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);
            // Pattern Data Load
            m_RefComp.Camera[iCamNo].SetSearchData(iTypeNo, pSData);
            // Mark Register
            CVisionPatternData CurData = m_RefComp.Camera[iCamNo].GetSearchData(iTypeNo);
            m_RefComp.System.ReloadModel(iCamNo, ref CurData);

            return SUCCESS;
        }

        /// <summary>
        /// Pattern Maching으로 Mark의 위치 확인
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <param name="SearchArea": Search Area Rectangle></param>
        /// <param name="ModelArea": Model Area Rectangle></param>
        /// <param name="ReferencePoint": Reference Point></param>
        /// <returns></returns>
        public int RegisterPatternMark(int iCamNo,
                                       string strModel,
                                       int iTypeNo,
                                       Rectangle SearchArea,
                                       Rectangle ModelArea)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            // 모델 갯수 보다 큰 경우 Err
            if (iTypeNo > DEF_USE_SEARCH_MARK_NO) return GenerateErrorCode(ERR_VISION_PATTERN_NUM_OVER);
            // Search Size 확인 
            if (SearchArea.Width <= DEF_SEARCH_MIN_WIDTH ||
               SearchArea.Height <= DEF_SEARCH_MIN_HEIGHT ||
               SearchArea.Width > m_RefComp.Camera[iCamNo].m_CamPixelSize.Width ||
               SearchArea.Height > m_RefComp.Camera[iCamNo].m_CamPixelSize.Height)
            {
                GenerateErrorCode(ERR_VISION_SEARCH_SIZE_OVER);
            }

            // 기존의 Mark 모델 Data를 연결함 (주소값으로 연결됨).
            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iTypeNo);

            // 등록할 Mark의 Size 및 위치를 설정함.
           //pSData.m_strFileName = strModel;
            pSData.m_rectModel = ModelArea;
            pSData.m_rectSearch = SearchArea;
            pSData.m_pointReference.X = ModelArea.Width/2;
            pSData.m_pointReference.Y = ModelArea.Height/2;

            // 기존에 등록된 모델이 있을 경우 삭제한다.
            if (pSData.m_milModel != MIL.M_NULL)
            {
                MIL.MpatFree(pSData.m_milModel);
                pSData.m_milModel = MIL.M_NULL;
                pSData.m_bIsModel = false;
            }

            // 설정한 Data로 Mark 모델을 등록한다.
            if(m_RefComp.System.RegisterMarkModel(iCamNo, ref pSData)==true)
            {
                pSData.m_bIsModel = true;
                // Model Register Grab Image Save
                string strPath = DEF_PATTERN_FILE;
                string strName = strModel + $"_Cam_{iCamNo}_Type_{iTypeNo}.bmp";
                // Image Save
                SaveModelImage(iCamNo, strPath, strName);

                // Image Path & Name apply
                pSData.m_strFilePath = strPath;
                pSData.m_strFileName = strName;
                
                return SUCCESS;
            }
            else
            {
                pSData.m_bIsModel = false;
                return GenerateErrorCode(ERR_VISION_PATTERN_REG_FAIL);
            }                 
        }
        
        /// <summary>
        /// Dispaly the registered Model Image or Model Area like below ;
        /// </summary>
        /// <param name="iCamNo" : Camera Number></param>
        /// <param name="iModelNo": Model Number></param>
        /// <param name="pHandle": Display 객체 Handle></param>
        /// <returns></returns>
        public int DisplayPatternImage(int iCamNo, int iModelNo, IntPtr pHandle)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);
            // Model No 확인
            if (isValidPatternMarkNo(iModelNo) == false) return GenerateErrorCode(ERR_VISION_PATTERN_NONE);
            
            // 저장된 Pattern 정보를 읽어옴
            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);

            // Data에 MIL 정보를 확인함.
            if (pSData.m_milModel ==MIL.M_NULL) return GenerateErrorCode(ERR_VISION_PATTERN_NONE);

            // Image Display 호출
            m_RefComp.View[iCamNo].DisplayImage(pSData.m_ModelImage, pHandle);
            
            return SUCCESS;
        }

        /// <summary>
        /// Search Area 설정
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <param name="SArea": Search Area Rectangle></param>
        /// <returns></returns>
        public int SetSearchArea(int iCamNo, int iModelNo, ref Rectangle SArea)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (SArea.Width <= DEF_SEARCH_MIN_WIDTH || SArea.Height <= DEF_SEARCH_MIN_HEIGHT ||
                SArea.Width > m_RefComp.Camera[iCamNo].m_CamPixelSize.Width || SArea.Height > m_RefComp.Camera[iCamNo].m_CamPixelSize.Height)
            {
                GenerateErrorCode(ERR_VISION_SEARCH_SIZE_OVER);
            }

            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);
            pSData.m_rectSearch = SArea;

            MIL.MpatSetPosition(pSData.m_milModel,
                            pSData.m_rectSearch.Left,
                            pSData.m_rectSearch.Top,
                            pSData.m_rectSearch.Width,
                            pSData.m_rectSearch.Height);

            MIL.MpatPreprocModel(MIL.M_NULL, pSData.m_milModel, MIL.M_DEFAULT);

            return SUCCESS;
        }

        /// <summary>
        /// 등록된 Mark Search.. GMF는 적용안됨.
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <param name="pPatResult" : Result value Check></param>
        /// <param name="bUseGMF" : Not Use></param>
        /// <returns></returns>
        public int RecognitionPatternMark(int iCamNo, int iModelNo, out CResultData pPatResult, bool bUseGMF = false)
        {
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) goto VISION_ERROR_GO;
            
            int iResult = 0;
            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);
            CResultData pSResult; 

            // 모델 생성 여부 확인
            if (pSData.m_bIsModel == false) goto VISION_ERROR_GO;

            // Mark Search 실행
            iResult = m_RefComp.System.SearchByNGC(iCamNo, pSData, out pSResult);
            if (iResult != SUCCESS)
            {
                pSResult.m_strResult = string.Format("Camera{0} : Model : {1} is Not Found! \n [sc:{2:0.00}%% Tm:{3:0.0}ms",
                                                    iCamNo,
                                                    iModelNo,
                                                    pSResult.m_dScore,
                                                    pSResult.m_dTime * 1000);

                goto VISION_ERROR_GO;

            }

            // 결과 내용 문자열에 저장
            pSResult.m_PixelPos = PositionToCenter(iCamNo, pSResult.m_PixelPos);

            pSResult.m_strResult = string.Format("-MK:{0} P_X:{1:0.00}  P_Y:{2:0.00}  \n        Sc:{3:0.00}%% Tm:{4:0.0}ms",
                                                iModelNo,
                                                pSResult.m_PixelPos.dX,
                                                pSResult.m_PixelPos.dY,
                                                pSResult.m_dScore,
                                                pSResult.m_dTime  *1000);
            if (pSResult.m_bSearchSuccess)
                pSResult.m_strResult = "OK" + pSResult.m_strResult;
            else
                pSResult.m_strResult = "NG" + pSResult.m_strResult;

            
            // Search 결과 대입
            pPatResult = pSResult;

            // Pattern Search Fail시 Image 저장
            if (pSResult.m_bSearchSuccess==false)
            {
                if(m_bSaveErrorImage)
                {
                    iResult = SaveImage(iCamNo,1,90);
                    if(iResult != SUCCESS)
                    {
                        return GenerateErrorCode(ERR_VISION_PATTERN_SEARCH_FAIL);
                    }
                }
            }
           
            return SUCCESS;
        // Vision Error 처리
        VISION_ERROR_GO:

            pPatResult = new CResultData();
            return GenerateErrorCode(ERR_VISION_PATTERN_SEARCH_FAIL);
        }

        private CPos_XY PositionToCenter(int iCam, CPos_XY pPos)
        {
            var mPos = new CPos_XY();
            double ImageCenterX = (double)m_RefComp.View[iCam].GetImageWidth() / 2;
            double ImageCenterY = (double)m_RefComp.View[iCam].GetImageHeight() / 2;

            mPos.dX = pPos.dX - ImageCenterX;
            mPos.dY = ImageCenterY - pPos.dY;

            return mPos;
        }

        /// <summary>
        /// SetSearchData : Search Data를 설정한다.
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <param name="pSearchData"></param>
        /// <returns></returns>
        public bool SetSearchData(int iCamNo, int iModelNo, CSearchData pSearchData)
        {
            return m_RefComp.Camera[iCamNo].SetSearchData(iModelNo,pSearchData);
        }

        public CSearchData GetSearchData(int iCamNo, int iModelNo)
        {
            if (m_RefComp.Camera == null) return new CSearchData();
            return m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);
        }

        public MIL_ID GetPatternImage(int iCamNo, int iModelNo)
        {
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);
            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);
            
            return pSData.m_milModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <param name="MaskRect": Rectangle for masking></param>
        /// <param name="ModelRect": GMF Model Rectangle></param>
        /// <param name="bMakeEndFlag": TRUE - Stop making mask image & 
        ///                                    Apply the Mask Image to GMF Search Context
        ///                             FALSE - Continue making mask image></param>
        /// <returns></returns>
        public int MaskImage(int iCamNo, int iModelNo, ref Rectangle MaskRect, ref Rectangle ModelRect, bool bMakeEndFlag)
        {
#if SIMULATION_VISION
                return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return SUCCESS;
        }

        /// <summary>
        /// Return Pattern Matching Result : Reference Point X value
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <returns>
        /// Position X value (double)
        /// </returns>
        public double GetSearchResultX(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_RefComp.Camera[iCamNo].GetResultData(iModelNo).m_PixelPos.dX;
        }



        /// <summary>
        /// Return Pattern Matching Result : Reference Point Y value
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <returns>
        /// Position Y value (double)
        /// </returns>
        public double GetSearchResultY(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_RefComp.Camera[iCamNo].GetResultData(iModelNo).m_PixelPos.dY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public double GetSearchScore(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_RefComp.Camera[iCamNo].GetResultData(iModelNo).m_dScore;
        }                

        /// <summary>
        /// Return Pattern Matching Result Model Rectangle
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <returns></returns>
        public Rectangle GetFindedModelRect(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return new Rectangle(0,0,0,0);
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return new Rectangle();

            if (!isValidPatternMarkNo(iModelNo))
                return new Rectangle(0, 0, 0, 0);

            return m_RefComp.Camera[iCamNo].GetResultData(iModelNo).m_rectFindedModel;
        }

        /// <summary>
        /// Return Pattern Matching Result Model Rectangle
        /// </summary>
        /// <param name="iCamNo": Camera Number></param>
        /// <param name="iModelNo": Model Mark Number></param>
        /// <returns></returns>
        public Rectangle GetSearchAreaRect(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return new Rectangle(0,0,0,0);
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return new Rectangle();

            if (!isValidPatternMarkNo(iModelNo))
                return new Rectangle(0, 0, 0, 0);

            return m_RefComp.Camera[iCamNo].GetSearchData(iModelNo).m_rectSearch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <returns></returns>
        public double GetSearchAcceptanceThreshold(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!isValidPatternMarkNo(iModelNo))
                return 0.0;

            return m_RefComp.Camera[iCamNo].GetSearchData(iModelNo).m_dAcceptanceThreshold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public int SetSearchAcceptanceThreshold(int iCamNo, int iModelNo, double dValue)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            if (!isValidPatternMarkNo(iModelNo))
                return GenerateErrorCode(ERR_VISION_PATTERN_NONE);

            if (dValue < 0.0 || dValue > 100.0)
                return GenerateErrorCode(ERR_VISION_PARAMETER_UNFIT); 

            CVisionPatternData pSData = m_RefComp.Camera[iCamNo].GetSearchData(iModelNo);
            if (pSData.m_milModel == MIL.M_NULL)
                return GenerateErrorCode(ERR_VISION_PATTERN_NONE); 

            pSData.m_dAcceptanceThreshold = dValue;
            MIL.MpatSetAcceptance(pSData.m_milModel, dValue);

            MIL_ID SourceImage = m_RefComp.View[iCamNo].GetImage();

            MIL.MpatPreprocModel(SourceImage, pSData.m_milModel, MIL.M_DEFAULT);

            return SUCCESS;
        }


        /// <summary>
        /// Related Edge Finder (Caliper Tool) Operations
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="dPosX"></param>
        /// <param name="dPosY"></param>
        /// <returns></returns>
        public int FindEdge(int iCamNo, out CEdgeData pEdgeData)
        {
            pEdgeData = new CEdgeData();
#if SIMULATION_VISION
            return SUCCESS;
#endif
            int iResult = 0;

            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            // Edge Find 지령
            iResult = m_RefComp.System.FindEdge(iCamNo,out pEdgeData);

            if (iResult != SUCCESS)
            {
                pEdgeData.m_strResult = "Edge Search Fail";
                return iResult;
            }
                        
            // 결과 값을 String으로 표현함.
            pEdgeData.m_strResult = "";
            for (int i = 0;  i < pEdgeData.m_iEdgeNum; i++)
            {
                pEdgeData.m_strResult = pEdgeData.m_strResult + string.Format("-Edge No:{0} P_X:{1:0.00}  P_Y:{2:0.00} \n",
                                                i+1,
                                                pEdgeData.EdgePos[i].dX,
                                                pEdgeData.EdgePos[i].dY);
            }
            pEdgeData.m_strResult = "Edge Search OK \n" + pEdgeData.m_strResult;
                
            
            return SUCCESS;
        }
        /// <summary>
        /// Edge Find의 Search 면적을 설정한다.
        /// 위치, 가로/세로 크기, 각도를 변경할 수 있다.
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="mPos"></param>
        /// <param name="mSize"></param>
        /// <param name="dAng"></param>
        /// <returns></returns>
        public int SetEdgeFinderArea(int iCamNo, Point mPos, double dWidth=400,double dHeight=3, double dAng=135.0)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            m_RefComp.System.SetEdgeFindParameter(mPos, dWidth, dHeight,dAng);

            return SUCCESS;
        }
        public int SetEdgeFinderArea(int iCamNo, double dWidth = 400, double dHeight = 3, double dAng = 135.0)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            Point mPos = new Point();
            // 위치는 영상의 가운데를 기준으로 한다.
            mPos.X = m_RefComp.View[iCamNo].GetImageWidth() / 2;
            mPos.Y = m_RefComp.View[iCamNo].GetImageHeight() / 2;

            m_RefComp.System.SetEdgeFindParameter(mPos, dWidth, dHeight, dAng);

            return SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <param name="dPolarity"></param>
        /// <returns></returns>
        public int SetEdgeFinderPolarity(int iCamNo, int iModelNo, double dPolarity)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="iModelNo"></param>
        /// <param name="dThreshold"></param>
        /// <returns></returns>
        public int SetEdgeFinderThreshold(int iCamNo, int iModelNo, double dThreshold)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return SUCCESS;
        }
        public int SetEdgeFinderNumOfResults(int iCamNo, int iModelNo, double dNumOfResults)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return SUCCESS;
        }
        public int SetEdgeFinderDirection(int iCamNo, int iModelNo, double dSearchDirection)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return SUCCESS;
        }

        public double GetEdgeFinderPolarity(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return SUCCESS;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return 0.0;
        }
        public double GetEdgeFinderThreshold(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return 0.0;
        }
        public double GetEdgeFinderNumOfResults(int iCamNo, int iModelNo)
        {
#if SIMULATION_VISION
            return 0.0;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return 0.0;
        }
        public double GetEdgeFinderDirection(int iCamNo, int iModelNo)
        {
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return GenerateErrorCode(ERR_VISION_SYSTEM_FAIL);

            return 0.0;
        }

        /// <summary>
        /// Camera 영상 Live Stop 설정
        /// </summary>
        /// <param name="iCamNo"></param>      
        public void HaltVideo(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.Camera[iCamNo].SetLive(false);            
        }


	     /// <summary>
         /// Camera 영상 Live 설정
         /// </summary>
         /// <param name="iCamNo"></param>
        public void LiveVideo(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.Camera[iCamNo].SetLive(true);
            
        }

        
        /// <summary>
        /// Clear Overlay Display
        /// </summary>
        /// <param name="iCamNo" : Camera number></param>
        public void ClearOverlay(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[iCamNo].ClearOverlay();
            
        }
        public void ClearOverlay()
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (m_iCurrentViewNum > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[m_iCurrentViewNum].ClearOverlay();
            
        }
        /// <summary>
        /// Rectangle on the Overlay Display	
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="rect"></param>

        public void DrawOverlayAreaRect(int iCamNo, Size rect)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[iCamNo].DrawBox(rect);
            //return 0;
        }

        public Size GetOverlayAreaRect(int iCamNo)
        {
            var sizeRect = new Size(0, 0);

            sizeRect.Width = m_iMarkROIWidth;
            sizeRect.Height = m_iMarkROIHeight;

            return sizeRect;

        }

        public void DrawOverlayAreaRect(Size rect)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (m_iCurrentViewNum > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[m_iCurrentViewNum].ClearOverlay();
            m_RefComp.View[m_iCurrentViewNum].DrawBox(rect);
            //return 0;
        }

        // Draw Grid On the Overlay Display
        public void DrawOverlayGrid(int iCamNo)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            //return 0;
        }

        // Draw Line On the Overlay Display
        public void DrawOverlayLine(int iCamNo, Point ptStart, Point ptEnd, int color)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            //return 0;
        }
        /// <summary>
        /// Cross Mark on the Overlay Display
        /// </summary>
        /// <param name="iCamNo"></param> 해당 Camera 번호
        /// <param name="iWidth"></param> Cross의 width설정
        /// <param name="iHeight"></param> Cross의 Height 설정
        /// <param name="center"></param>  Cross의 위치 설정
        // 
        public void DrawOverlayCrossMark(int iCamNo, int iWidth, int iHeight,Point center)    //, int color = 1) ;
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[iCamNo].DrawCrossMark(center,iWidth,iHeight);   
                    
        }

        public void DrawOverlayCrossMark(int iWidth, int iHeight, Point center)    //, int color = 1) ;
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (m_iCurrentViewNum > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[m_iCurrentViewNum].DrawCrossMark(center, iWidth, iHeight);

        }

        // Draw Text on the View Image Display
        public void DrawOverlayText(int iCamNo, string strText, Point pointText)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            if (iCamNo > DEF_MAX_CAMERA_NO) return;

            m_RefComp.View[m_iCurrentViewNum].DrawString(strText, pointText);

            //return 0;
        }

        /// <summary>
        /// Wafer Cutting의 기준이 되는 HairLine을 Draw한다.
        /// 가운데 Line과 폭 (Width)의 2개 Line 총 3개의 Line을 Draw한다.
        /// </summary>
        /// <param name="iCamNo"></param>
        /// <param name="Width"></param>
        public void DrawOverLayHairLine(int iCamNo, int Width)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            // Hair Line의 제한값 설정
            if (Width < DEF_HAIRLINE_MIN && Width > DEF_HAIRLINE_MAX) return;

            m_iHairLineWidth = Width;

            // Display할 객체가 연결되어 있는지 확인
            if (m_RefComp.View[iCamNo].IsLocalView())
            {
                m_RefComp.View[iCamNo].ClearOverlay();
                m_RefComp.View[iCamNo].DrawHairLine(Width);
            }
            
        }

        public void DrawOverLayHairLine(int Width=0)
        {
#if SIMULATION_VISION
            return;
#endif
            // Vision System이 초기화 된지를 확인함
            if (m_bSystemInit == false) return;

            // Hair Line의 제한값 설정
            if (Width < DEF_HAIRLINE_MIN && Width > DEF_HAIRLINE_MAX) m_iHairLineWidth = Width;
            
            // Display할 객체가 연결되어 있는지 확인
            if (m_RefComp.View[m_iCurrentViewNum].IsLocalView())
            {
                m_RefComp.View[m_iCurrentViewNum].ClearOverlay();
                m_RefComp.View[m_iCurrentViewNum].DrawHairLine(Width);
            }
        }
        public void NarrowHairLine()
        {
            m_iHairLineWidth--;
            DrawOverLayHairLine(m_iHairLineWidth);
        }

        public void WidenHairLine()
        {
            m_iHairLineWidth++;
            DrawOverLayHairLine(m_iHairLineWidth);
        }
        public void NarrowRoiWidth()
        {
            m_iMarkROIWidth--;
            Size recSize= new Size(m_iMarkROIWidth, m_iMarkROIHeight);
            DrawOverlayAreaRect(recSize);
        }

        public void WidenRoiWidth()
        {
            m_iMarkROIWidth++;
            Size recSize = new Size(m_iMarkROIWidth, m_iMarkROIHeight);
            DrawOverlayAreaRect(recSize);
        }

        public void NarrowRoiHeight()
        {
            m_iMarkROIHeight--;
            Size recSize = new Size(m_iMarkROIWidth, m_iMarkROIHeight);
            DrawOverlayAreaRect(recSize);
        }

        public void WidenRoiHeight()
        {
            m_iMarkROIHeight++;
            Size recSize = new Size(m_iMarkROIWidth, m_iMarkROIHeight);
            DrawOverlayAreaRect(recSize);
        }

    }
}
