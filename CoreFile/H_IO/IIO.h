#ifndef IIO_H
#define IIO_H

#include "DefIO.h"

/**
 * This is an interface class for the IO component.
 * �� Hischer Board�� �ϳ��� I/O Component�� �����Ѵ� (�ϳ��� CHilscherDnet ��ü ����)
 * @author Jeongseung Moon
 * version 1.0
 * @interface
 */
class IIO 
{
public:  
	/************************************************************************/
	/* �Ҹ���                                                                     */
	/************************************************************************/
	virtual ~IIO() {};
	
    /**
     * Hilscher Board���� Communication�� ���� Driver�� Open�ϸ�, Board�� �ʱ�ȭ�ϰ� ��� ��� ���°� �ǰ� �Ѵ�.
     * @precondition �� �Լ��� ��ü�� �������� ��ó�� �ѹ��� �����Ѵ�. ���� �������� ���� �������� ���ƾ��Ѵ�.
     * @postcondition Hilscher Board�� ����� ���� �غ� ���°� �ȴ�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int Initialize() = 0;

    /**
     * I/O Device�� Digital Status (Bit) ��  �о�帰��.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
	 * @param pbVal    : IO ��
     * @return  0      : SUCCESS
	            else   : Device \Error �ڵ� 
     */
    virtual int GetBit(unsigned short usIOAddr, BOOL *pbval)  = 0;

    /**
     * Hilscher Board���� Communication�� �����ϰ� Device Driver�� Close�Ѵ�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize  �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @postcondition Hilscher Board�� ��� ����
      * @return 0 = Success, �׿� = Error Number
     */
    virtual int Terminate()  = 0;

    /**
     * I/O Device�� Digital Status (Bit) �� �о�鿩 bit ���� �ƱԸ�Ʈ�� �����Ѵ�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : ���� 1 ��, FALSE : ���� 0 ��
     * @return  0      : SUCCESS
	            else   : Device Error �ڵ� 
     */
    virtual int IsOn(unsigned short usIOAddr, BOOL *pbVal) = 0;

   /**
     * I/O Device�� Digital Status (Bit) �� �о�鿩 bit ���� �ƱԸ�Ʈ�� �����Ѵ�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
	 * @param pbVal    : TRUE : ���� 0 ��, FALSE : ���� 1 ��
     * @return  0      : SUCCESS
	            else   : Device Error �ڵ� 
     */
    virtual int IsOff(unsigned short usIOAddr, BOOL *pbVal) = 0;

    /**
     * Output Device�� On Command (Bit = 1) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputOn(unsigned short usIOAddr) = 0;

    /**
     * Output Device�� Off Command (Bit = 0) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputOff(unsigned short usIOAddr) = 0;

    /**
     * Output Device�� Digital Status�� Set�̸� (Bit = 0), Output Device�� On Command (Bit = 1) �� ������,
     * Output Device�� Digital Status�� Clear�̸� (Bit = 1), Output Device�� Off Command (Bit = 0) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : IO Address
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputToggle(unsigned short usIOAddr) = 0;

    /**
     * ���ӵ� 8���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue pointer�� �Ѱ��ش�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : ���ӵ� 8���� IO Address�� �����ϴ� IO Address
     * @param pcValuse : ���ӵ� 8���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue�� �����Ѵ�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int GetByte(unsigned short usIOAddr, BYTE & pcValue) = 0;

    /**
     * ���ӵ� 8���� IO Address�� ������ Output Device�鿡 On or Off Command�� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : ���ӵ� 8���� IO Address�� �����ϴ� IO Address
     * @param pcValuse : Output Device�� ���� Command�� �����ϰ� �ִ� �����̴�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int PutByte(unsigned short usIOAddr, BYTE pcValue) = 0;

    /**
     * ���ӵ� 16���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue pointer�� �Ѱ��ش�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : ���ӵ� 16���� IO Address�� �����ϴ� IO Address
     * @param pwValuse : ���ӵ� 16���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue�� �����Ѵ�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int GetWord(unsigned short usIOAddr, WORD & pwValue) = 0;

    /**
     * ���ӵ� 16���� IO Address�� ������ Output Device�鿡 On or Off Command�� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param usIOAddr : ���ӵ� 16���� IO Address�� �����ϴ� IO Address
     * @param pwValuse : Output Device�� ���� Command�� �����ϰ� �ִ� �����̴�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int PutWord(unsigned short usIOAddr, WORD pwValue) = 0;

    /**
     * I/O Device�� Digital Status (Bit) ��  �о�帰��.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO ��
     * @return  0      : SUCCESS
	            else   : Device \Error �ڵ� 
     */
    virtual int GetBit(CString strIOAddr, BOOL *pbVal) = 0;

    /**
     * I/O Device�� Digital Status (Bit) �� �о�鿩 Bit = 1�̸�, TRUE(1)�� Return�ϰ�, Bit = 0�̸� FALSE(0)�� Return�Ѵ�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
	 * @param pbVal    : IO ��
     * @return  0      : SUCCESS
	            else   : Device \Error �ڵ� 
     */
    virtual int IsOn(CString strIOAddr, BOOL *pbVal) = 0;

    /**
     * Output Device�� On Command (Bit = 1) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputOn(CString strIOAddr) = 0;

    /**
     * Output Device�� Off Command (Bit = 0) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputOff(CString strIOAddr) = 0;

    /**
     * Output Device�� Digital Status�� Set�̸� (Bit = 0), Output Device�� On Command (Bit = 1) �� ������,
     * Output Device�� Digital Status�� Clear�̸� (Bit = 1), Output Device�� Off Command (Bit = 0) �� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : IO Address String (ex, "1000:START_SW")
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int OutputToggle(CString strIOAddr) = 0;

    /**
     * ���ӵ� 8���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue pointer�� �Ѱ��ش�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : ���ӵ� 8���� IO Address�� �����ϴ� IO Address�� String Type (ex, "1000:START_SW")
     * @param pcValuse : ���ӵ� 8���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue�� �����Ѵ�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int GetByte(CString strIOAddr, BYTE & pcValue) = 0;

    /**
     * ���ӵ� 8���� IO Address�� ������ Output Device�鿡 On or Off Command�� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : ���ӵ� 8���� IO Address�� �����ϴ� IO Address�� String Type (ex, "1000:START_SW")
     * @param pcValuse : Output Device�� ���� Command�� �����ϰ� �ִ� �����̴�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int PutByte(CString strIOAddr, BYTE pcValue) = 0;

    /**
     * ���ӵ� 16���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue pointer�� �Ѱ��ش�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : ���ӵ� 16���� IO Address�� �����ϴ� IO Address�� String Type (ex, "1000:START_SW")
     * @param pwValuse : ���ӵ� 16���� IO Address�� ������ Input Device ���� Digital Status�� �о�鿩 pcValue�� �����Ѵ�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int GetWord(CString strIOAddr, WORD & pwValue) = 0;

    /**
     * ���ӵ� 16���� IO Address�� ������ Output Device�鿡 On or Off Command�� ������.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @param strIOAddr : ���ӵ� 16���� IO Address�� �����ϴ� IO Address�� String Type (ex, "1000:START_SW")
     * @param pwValuse : Output Device�� ���� Command�� �����ϰ� �ִ� �����̴�.
     * @return 0 = Success, �׿� = Error Number
     */
    virtual int PutWord(CString strIOAddr, WORD pwValue) = 0;

    /**
     * Incoming Buffer�� Update�ϰ�, Outgoing Buffer�� ������ Physical I/O�� �����ϴ� IOThread�� Run�Ѵ�.
     * @precondition �� �Լ��� �����ϱ� ���� initialize �Լ��� �̸� ����Ǿ���� �Ѵ�.
     * @postcondition Incoming Buffer�� Update�ϰ�, Outgoing Buffer�� ������ Physical I/O�� �����ϴ� IOThread�� Run�Ѵ�.
     */
    virtual void RunIOThread() = 0;

	/**
	 * Master ��� �� Slave ��� ���� ������ ���´�.
	 *
	 * @param  DnStatus : �����Ϳ� 64���� Slave�� ���� ���� ���� ����ü
	 * @return 0		= ��� ����
			   others	= �ϳ��� ����
	 */
	virtual int DnStatusGet(DN_STATUS DnStatus) = 0;


/*----------- Component ����  -----------------------*/

   /**
     * Error Code Base�� �����Ѵ�. 
	 *
	 * @param	iErrorBase : (OPTION=0) ������ Error Base ��
     */
    virtual void SetErrorBase(int iErrorBase = 0) = 0;

    /**
     * Error Code Base�� �д´�. 
	 *
	 * @return	int : Error Base ��
     */
    virtual int GetErrorBase(void) const = 0;


   /**
     * Object ID�� �����Ѵ�. 
	 *
	 * @param	iObjectID : ������ Object ID ��
     */
    virtual void SetObjectID(int iObjectID) = 0;

    /**
     * Object ID�� �д´�. 
	 *
	 * @return	int : Object ID ��
     */
    virtual int GetObjectID(void) = 0;

	/** 
	 * Log manager�� ��ȯ�Ѵ�.
	 *
	 * @return	MLog* : ��ȯ�� Log Manager Pointer
	 */
	virtual MLog* GetLogManager() = 0;	

	/** 
	 * Log Class�� ��ü Pointer�� �����Ѵ�.
	 *
	 * @param		*pLogObj: ������ Log Class�� ��ü Pointer
	 * @return		Error Code : 0 = Success, �� �� = Error
	 */
	virtual int SetLogObject(MLog *pLogObj) = 0;

   /**
     * Log File�� ���õ� attribute�� �����Ѵ�.
     *
	 * @param	iObjectID : ObjectID
     * @param	strFileName : file path �� file name�� ������ string
     * @param	ucLevel : log level ���� bitwise ����
     * @param	iDays : (OPTION=30) ���� set�Ǿ� �ִ� log file ������
     * @return	Error Code : 0 = Success, �� �� = Error
     */
    virtual int SetLogAttribute (int iObjectID, CString strFullFileName, BYTE ucLevel, int iDays = 30) = 0;
    /**
     * ������ Log file�� �����Ѵ�.
     *
     * @return	Error Code : 0 = Success, �� �� = Error
     */
    virtual int DeleteOldLogFiles (void) = 0;

	/** 
	 * Component�� Error Code Base�� ��ȯ�Ѵ�.
	 *
	 * @param		Error Code: ObjectID + Error Base 
	 * @return		ErrorBase�� ���ŵ� Component Error Code 
	 */
	virtual int DecodeError(int iErrCode) = 0;

};
#endif //IIO_H
