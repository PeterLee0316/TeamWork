#ifndef DEF_IO_H
#define DEF_IO_H

#include "MLog.h"
#include "CIFUSER.h"

const	int		DEF_IO_SUCCESS				= 0;

const	int		ERR_IO_SUCCESS				= 0;
const	int		ERR_IO_LOG_NULL_POINTER		= 1;
const	int		ERR_IO_NULL_DATA			= 2;
const	int		ERR_IO_UPDATE_FAIL			= 3;
const	int		ERR_IO_MODULE_FAIL			= 4;
const	int		ERR_IO_DRIVER_FAIL			= 5;

const	char	STS_NONE					= 0;		
const	char	STS_NORMAL					= 1;
const	char	STS_FAIL					= 2;
const	char	STS_STOP					= 3;	

typedef
 /**
  * DeviceNet �� ���¸� �÷� �ֱ� ���� ����Ÿ �����̴�. 
  */
 struct DN_STATUS {
	/** Master ����� ���� ǥ�� */
	char dnm_status;
	/** 4����Ʈ Align */
	char reserved[3];
	/** 64���� Slave������ ������ ���� ǥ�� */
	char dns_status[MAX_DEVICE];
} DN_STATUS;

/** IO Component�� ������ Mechanical Component List */
typedef struct tagSIORefCompList
{
;
} SIORefCompList, *pSIORefCompList;

typedef struct tagSIOData
{
;
} SIOData, *pSIOData;

#endif //DEF_IO_H