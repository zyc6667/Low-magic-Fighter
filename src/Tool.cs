using System;

namespace Low_magic_Fighter
{
    public static class Tool 
    {
        //绝对值
        public static int GetAbs(int num)
        {
            if (num >= 0) return num;
            else return -num;
        }

    }

    //环形缓冲区
    public class CircularBuffer<T>
    {
        private T[] buffer; // 存储数据的数组
        private int head; // 写入位置
        private int tail; // 读取位置
        private int maxSize; // 缓冲区最大容量
        private int count; // 当前数据数量

        public CircularBuffer(int size)
        {
            maxSize = size;
            buffer = new T[size];
            head = 0;
            tail = 0;
            count = 0;
        }

        // 添加数据到缓冲区
        public void Add(T item)
        {
            buffer[head] = item; // 将新数据写入头部
            head = (head + 1) % maxSize; // 更新头部索引
            if (count < maxSize)
            {
                count++; // 如果缓冲区未满，增加计数
            }
            else
            {
                tail = (tail + 1) % maxSize; // 更新尾部索引，覆盖旧数据（因为tail始终指向最旧的数据）
            }
        }

        // 从缓冲区读取数据
        public T Read()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Buffer is empty");
            }
            T item = buffer[tail]; // 读取尾部数据
            tail = (tail + 1) % maxSize; // 更新尾部索引
            count--; // 减少计数
            return item;
        }

        // 返回当前数据数量
        public int Count => count;

        // 将缓冲区中的数据转换为数组
        public T[] ToArray()
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = buffer[(tail + i) % maxSize]; // 返回有效数据
            }
            return result;
        }
    }

}